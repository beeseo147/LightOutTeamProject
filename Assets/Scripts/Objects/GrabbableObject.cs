using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public enum Weight
{
    Light,
    Medium,
    Heavy
}

/// <summary>
/// 붙이기만 하면 Grab 가능해지는 컴포넌트.
/// 요청한 컴포넌트들이 없으면 에디터,런타임 모두에서 경고하고 비활성화.
/// </summary>
[RequireComponent(typeof(XRGrabInteractable), typeof(Rigidbody))]
public class GrabbableObject : MonoBehaviour
{
    [Header("Optional Hand Pose Profile")]
    public HandPoseProfileSO handPose;

    [Header("Optional Weight Category")]
    public Weight weight = Weight.Heavy;

    //[Header("Push by external collision")]
    //public bool isPushable = false;

    XRGrabInteractable grab;
    Rigidbody rb;
    bool originalKinematic;
    RigidbodyConstraints originalConstraints;

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

        ApplyWeightSettings();

        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.isKinematic = false;
        rb.useGravity = true;

        /* ---------- Grab Setting ---------- */
        /* XR Grab 세팅 (던지기 물체는 throw 켜기) */
        grab.movementType = XRBaseInteractable.MovementType.VelocityTracking;
        grab.throwOnDetach = true;
        grab.attachEaseInTime = 0.25f; // attach slowly

        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        if (handPose)
        {
            var controller = args.interactorObject.transform.GetComponentInChildren<HandAnimationController>();

            if (controller)
            {
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