using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// 붙이기만 하면 Rotation 가능해지는 컴포넌트.
/// 요청한 컴포넌트들이 없으면 에디터,런타임 모두에서 경고하고 비활성화.
/// </summary>
[RequireComponent(typeof(XRGrabInteractable), typeof(Rigidbody))]
[RequireComponent(typeof(PhotonView))]

public class RotatableObject : MonoBehaviourPun, IPunObservable
{
    public HandPoseProfileSO handPose;

    [Header("Rotation Settings")]
    public Axis rotationAxis = Axis.Y;
    public float minAngle = -45f;
    public float maxAngle = 45f;
    public bool useLimits = true;

    PhotonTransformView ptv;
    XRGrabInteractable grab;
    Rigidbody rb;
    Quaternion initialObjRot, initialHandRot;
    Vector3 initialHandVec;
    float smoothedAngle = 0f;
    /* Networking */
    float netFinalAngle;
    Vector3 netRotAxis;

    void Awake()
    {
        ptv = GetComponent<PhotonTransformView>();
        grab = GetComponent<XRGrabInteractable>() ?? gameObject.AddComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
        /* ---------- Rigidbody Setting ---------- */
        if (rb)
        {
            // No Collision
            rb.isKinematic = true;
            grab.throwOnDetach = false;
            // No Move
            rb.constraints = RigidbodyConstraints.FreezePosition;

            // Fix Rotation Axis
            if (rotationAxis == Axis.X)
                rb.constraints |= RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
          
            else if (rotationAxis == Axis.Y)
                rb.constraints |= RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
          
            else if (rotationAxis == Axis.Z)
                rb.constraints |= RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        }

        /* ---------- Grab Setting ---------- */
        grab.trackPosition = false;
        grab.trackRotation = true;
        grab.movementType = XRBaseInteractable.MovementType.Kinematic;

        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        if (!photonView.IsMine)
        {
            photonView.RequestOwnership();
        }

        if (handPose)
        {
            var controller = args.interactorObject.transform.GetComponentInChildren<HandAnimationController>();

            if (controller && handPose)
            {
                photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
                controller.SetOverrideState(handPose.poseState);
            }
        }

        // Save : Vector, Rotation Origin 
        initialHandVec = HandRefVector(args.interactorObject.transform).normalized;
        initialObjRot = transform.rotation;
        initialHandRot = args.interactorObject.transform.rotation;

        // Directly Rotate
        grab.trackRotation = false;
    }

    void OnRelease(SelectExitEventArgs args)
    {
        // init HandPose
        var controller = args.interactorObject.transform.GetComponentInChildren<HandAnimationController>();
        if (controller)
        {
            controller.ClearOverride();
            photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
        }

            grab.trackRotation = true;
    }

    void Update()
    {
        if (!grab || !grab.isSelected)
            return;

        Transform handTf = grab.interactorsSelecting[0].transform;

        /* 1) 현재 컨트롤러 기준 벡터를 축 직교 평면에 투영 */
        netRotAxis = AxisVector(rotationAxis);
        Vector3 grabVec = ProjectOnPlane(initialHandVec, netRotAxis).normalized; // Origin Vector
        Vector3 curVec  = ProjectOnPlane(HandRefVector(handTf), netRotAxis).normalized;

        // 2) Get Degree : -180 ~ 180
        float rawAngle = Vector3.SignedAngle(grabVec, curVec, netRotAxis);

        // 3) Ignore : Up to dead degrees
        const float dead = 5f;
        if (Mathf.Abs(rawAngle) < dead)
            rawAngle = 0f;

        // 4) Lerp 12% : Old Degree/New Degree (To Smoothly)
        const float lerpK = 0.12f;
        smoothedAngle = Mathf.Lerp(smoothedAngle, rawAngle, lerpK);

        // 5) Limit : Rotation Range
        netFinalAngle = useLimits ?
            Mathf.Clamp(smoothedAngle, minAngle, maxAngle) : smoothedAngle;

        transform.rotation = initialObjRot * Quaternion.AngleAxis(netFinalAngle, netRotAxis);
    }
    public enum Axis { X, Y, Z }
    Vector3 AxisVector(Axis ax)
    {
        return ax switch
        {
            Axis.X => Vector3.right,
            Axis.Y => Vector3.up,
            Axis.Z => Vector3.forward,
            _ => Vector3.up
        };
    }

    /* ② 컨트롤러에서 “각 축에 맞춰” 돌릴 때 기준이 될 벡터 선택 */
    Vector3 HandRefVector(Transform hand) => rotationAxis switch
    {
        Axis.Y => hand.right,
        Axis.X => hand.up,   
        Axis.Z => hand.up,
        _ => hand.forward,
    };

    /* 한 벡터를 평면(법선 n)에 투영 */
    static Vector3 ProjectOnPlane(Vector3 v, Vector3 n) => v - Vector3.Dot(v, n) * n;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(netFinalAngle);
            stream.SendNext(netRotAxis);
        }
        else
        {
            netFinalAngle = (float)stream.ReceiveNext();
            netRotAxis = (Vector3)stream.ReceiveNext();
        }
    }
}