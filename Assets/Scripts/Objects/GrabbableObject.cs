using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;
using System;

public enum Weight
{
    Light,
    Medium,
    Heavy
}

/// <summary>
/// ���̱⸸ �ϸ� Grab ���������� ������Ʈ.
/// ��û�� ������Ʈ���� ������ ������,��Ÿ�� ��ο��� ����ϰ� ��Ȱ��ȭ.
/// </summary>
[RequireComponent(typeof(XRGrabInteractable), typeof(Rigidbody))]
[RequireComponent(typeof(PhotonView), typeof(PhotonTransformView))]

// typeof(PhotonTransformView)
public class GrabbableObject : MonoBehaviourPun
{
    [Header("Optional Hand Pose Profile")]
    public HandPoseProfileSO handPose;

    [Header("Optional Weight Category")]
    public Weight weight = Weight.Heavy;

    [Header("Clickable Setting")]
    public bool isClickable = false;
    [SerializeField] UnityEvent OnclickEvent;
    [SerializeField] UnityEvent OffclickEvent;

    protected XRGrabInteractable grab;
    Rigidbody rb;
    RigidbodyConstraints originalConstraints;
    bool originalKinematic;
    bool isClicked = false;
    bool isPickedUp = false;

    [Header("OutLine")]
    [SerializeField] Material outlineMT;
    MeshRenderer rend;
    List<Material> materialList = new List<Material>();

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (GetComponent<Collider>() == null)
            Debug.LogError($"[GrabbableObject] {name} : ! No Collider !", this);
    }
#endif

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        rb.interpolation = RigidbodyInterpolation.Interpolate;
        ApplyWeightSettings();

        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.isKinematic = false;
        rb.useGravity = true;

        /* ---------- Grab Setting ---------- */
        grab.movementType = XRBaseInteractable.MovementType.VelocityTracking;
        grab.throwOnDetach = true;
        grab.attachEaseInTime = 0.15f; // attach slowly

        grab.hoverEntered.AddListener(OnHoverEnter);
        grab.hoverExited.AddListener(OnHoverExit);
        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnRelease);

        //if (isClickable)
        //    handPose.poseState = HandState.Point;

        rend = GetComponent<MeshRenderer>();
    }

    private void OnHoverEnter(HoverEnterEventArgs arg0)
    {
        DrawOutLine();
    }

    private void OnHoverExit(HoverExitEventArgs arg0)
    {
        EraseOutLine();
    }

    void DrawOutLine()
    {
        if (rend)
        {
            materialList.Clear();
            materialList.AddRange(rend.sharedMaterials);
            if (!materialList.Contains(outlineMT))
            {
                materialList.Add(outlineMT);
                rend.sharedMaterials = materialList.ToArray();
            }
        }
    }

    void EraseOutLine()
    {
        if (rend)
        {
            materialList.Clear();
            materialList.AddRange(rend.sharedMaterials);
            if (materialList.Contains(outlineMT))
            {
                materialList.Remove(outlineMT);
                rend.sharedMaterials = materialList.ToArray();
            }
        }
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        if (isPickedUp)
            return;

        isPickedUp = true;

        if (handPose)
        {
            if (isClickable)
            {
                if (!isClicked)
                {
                    OnclickEvent.Invoke();      // Light On
                    isClicked = true;
                }
                else
                {
                    OffclickEvent.Invoke();     // Light Off
                    isClicked = false;
                }
            }

            var controller = args.interactorObject.transform.GetComponentInChildren<HandAnimationController>();

            if (controller)
            {
                photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
                controller.SetOverrideState(handPose.poseState);
                grab.movementType = XRBaseInteractable.MovementType.Kinematic;
                // Directly Move Setting : No Physics
                //rb.isKinematic = true;
            }
        }
    }

    void OnRelease(SelectExitEventArgs args)
    {
        var controller = args.interactorObject.transform.GetComponentInChildren<HandAnimationController>();

        isPickedUp = false;

        if (controller)
        {
            controller.ClearOverride();
            grab.movementType = XRBaseInteractable.MovementType.VelocityTracking;
            // Directly Move Reset
            //rb.constraints = originalConstraints;
            //rb.isKinematic = originalKinematic;
        }
    }

    // Set the PhysicsMaterial appropriate to weight
    void ApplyWeightSettings()
    {
        // 1) PhysicMaterial Setting
        //string path = weight switch
        //{
        //    Weight.Light => "PhysMat/PM_Light_Weight",
        //    Weight.Medium => "PhysMat/PM_Medium_Weight",
        //    Weight.Heavy => "PhysMat/PM_Heavy_Weight",
        //    _ => "PhysMat/PM_Heavy_Weight"
        //};

        //PhysicMaterial mat = Resources.Load<PhysicMaterial>(path);
        //if (mat == null)
        //{
        //    Debug.LogWarning($"{name} : {path} No Found Material -> Set Defualt");
        //    return;
        //}
        //foreach (var col in GetComponentsInChildren<Collider>())
        //    col.sharedMaterial = mat;

        // 2) Rigidbody Setting
        switch (weight)
        {
            case Weight.Light:
                rb.mass = 1f;
                rb.drag = 0.1f;
                rb.angularDrag = 0.5f;
                rb.maxDepenetrationVelocity = 5f;
                grab.throwVelocityScale = 1f;
                break;

            case Weight.Medium:
                rb.mass = 30f;
                rb.drag = 0.001f;
                rb.angularDrag = 3f;
                rb.maxDepenetrationVelocity = 2f;
                grab.throwVelocityScale = 0.4f;
                break;

            case Weight.Heavy:
                rb.mass = 100f;
                rb.drag = 0.001f;
                rb.angularDrag = 5f;
                rb.maxDepenetrationVelocity = 0.5f;
                grab.throwVelocityScale = 0.2f;
                break;
        }
    }
}