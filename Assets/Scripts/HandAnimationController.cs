using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void SetHandState(HandState state)
    {
        // 전부 false 초기화
        handAnimator.SetBool("bNeutral", false);
        handAnimator.SetBool("bPoint", false);
        handAnimator.SetBool("bHoldFlashlight", false);
        handAnimator.SetBool("bFormFist", false);

        switch (state)
        {
            case HandState.Default:
                handAnimator.SetBool("bNeutral", true);
                break;
            case HandState.Point:
                handAnimator.SetBool("bPoint", true);
                break;
            case HandState.GrabLight:
                handAnimator.SetBool("bHoldFlashlight", true);
                break;
            case HandState.GrabHard:
                handAnimator.SetBool("bFormFist", true);
                break;
        }
    }
}
