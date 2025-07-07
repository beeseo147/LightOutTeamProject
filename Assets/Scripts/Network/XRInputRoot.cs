using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRInputRoot : MonoBehaviourPun
{
    [SerializeField] GameObject inputRig;     // XR Origin, Controllers 등
    [SerializeField] MonoBehaviour[] scripts; // Grab, Locomotion 등 입력 스크립트들

    void Awake()
    {
        //if (!photonView.IsMine)
        //{
        //    // Remote : Camera X Input X
        //    inputRig.SetActive(false);
        //    foreach (var s in scripts) s.enabled = false;
        //}
        //else
        //{
        //    // Local : Camera O
        //    GetComponentInChildren<Renderer>(true).enabled = false;
        //}
    }
}