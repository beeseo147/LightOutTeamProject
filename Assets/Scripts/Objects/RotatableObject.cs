using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// 붙이기만 하면 Rotation 가능해지는 컴포넌트.
/// 요청한 컴포넌트들이 없으면 에디터,런타임 모두에서 경고하고 비활성화.
/// </summary>
[RequireComponent(typeof(XRGrabInteractable), typeof(Rigidbody))]

public class RotatableObject : MonoBehaviour
{
    public HandPoseProfileSO handPose;

    [Header("Rotation Settings")]
    public Axis rotationAxis = Axis.Y;
    public float minAngle = -45f;
    public float maxAngle = 45f;
    public bool useLimits = true;

    XRGrabInteractable grab;
    Rigidbody rb;
    Quaternion initialObjRot, initialHandRot;

    void Awake()
    {
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
            // FreezePosition : X, Z
            rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
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
        if (handPose)
        {
            var controller = args.interactorObject.transform.GetComponentInChildren<HandAnimationController>();

            if (controller && handPose)
                controller.SetOverrideState(handPose.poseState);
        }

        // Save : Rotation Origin 
        initialObjRot = transform.rotation;
        initialHandRot = args.interactorObject.transform.rotation;

        // Directly Rotate
        grab.trackRotation = false;
    }

    void OnRelease(SelectExitEventArgs args)
    {
        // init HandPose
        var controller = args.interactorObject.transform.GetComponentInChildren<HandAnimationController>();
        if (controller) controller.ClearOverride();

        grab.trackRotation = true;
    }

    void Update()
    {
        if (!grab || !grab.isSelected)
            return;

        var hand = grab.interactorsSelecting[0].transform.rotation;
        Quaternion delta = hand * Quaternion.Inverse(initialHandRot);

        // angle calculation
        Vector3 euler = (delta * Vector3.forward).normalized;
        float angle = Vector3.SignedAngle(euler, Vector3.forward, AxisVector(rotationAxis));

        if (useLimits) angle = Mathf.Clamp(angle, minAngle, maxAngle);

        transform.rotation = initialObjRot * Quaternion.AngleAxis(angle, AxisVector(rotationAxis));
    }

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

    public enum Axis { X, Y, Z }
}