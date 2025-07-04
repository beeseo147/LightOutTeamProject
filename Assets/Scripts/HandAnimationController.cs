using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum HandState
{
    Default,
    Point,
    GrabLight,
    GrabHard
}

public class HandAnimationController : MonoBehaviour
{
    public Animator handAnimator;
    public InputActionProperty triggerAction;
    public InputActionProperty gripAction;

    [Header("Threshords")]
    [Range(0, 1)] public float triggerThreshold = 0.7f;
    [Range(0, 1)] public float gripThreshold = 0.3f;

    HandState current = HandState.Default;
    private bool overrideByObject = false; // picked up object
    private void OnEnable()
    {
        triggerAction.action.Enable();
        gripAction.action.Enable();
    }

    private void OnDisable()
    {
        triggerAction.action.Disable();
        gripAction.action.Disable();
    }

    private void Update()
    {
        // If you have already picked it up, Ignore input
        if (overrideByObject) return;

        float trigger = triggerAction.action.ReadValue<float>();
        float grip = gripAction.action.ReadValue<float>();

        if (trigger > triggerThreshold && grip > gripThreshold)
        {
            SetHandState(HandState.GrabHard);
        }
        else if (grip > gripThreshold)
        {
            SetHandState(HandState.GrabLight);
        }
        else if (trigger > triggerThreshold)
        {
            SetHandState(HandState.Point);
        }
        else { SetHandState(HandState.Default); }
    }

    public void SetHandState(HandState state)
    {
        if(state == current) return;
        current = state;

        handAnimator.SetBool("bNeutral", state == HandState.Default);
        handAnimator.SetBool("bPoint", state == HandState.Point);
        handAnimator.SetBool("bHoldFlashlight", state == HandState.GrabLight);
        handAnimator.SetBool("bFormFist", state == HandState.GrabHard);
    }


    /* ---------- For external access ---------- */
    public void SetOverrideState(HandState pose)
    {
        overrideByObject = true;
        SetHandState(pose); // Setting pose : picked Object's HandPose
    }

    public void ClearOverride()
    {
        overrideByObject = false;
        //SetHandState(HandState.Default);
    }
}
