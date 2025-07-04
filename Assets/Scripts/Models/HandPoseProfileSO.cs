using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "VR/HandPoseProfile")]
public class HandPoseProfileSO : ScriptableObject
{
    // HandAnimsInput Animator's (bool)Pose State
    public HandState poseState;
}