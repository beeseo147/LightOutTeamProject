using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class PlayerSetup : MonoBehaviourPun
{
    [SerializeField] GameObject xrOrigin;        // XR Origin  Ѹ 
    [SerializeField] GameObject avatarVisuals;
    //[SerializeField] GameObject cameraOffset;
    //[SerializeField] GameObject locomotion;
    //[SerializeField] GameObject leftHandModel;
    //[SerializeField] GameObject rightHandModel;

    void Awake()
    {
        if (photonView.IsMine)
        {
            xrOrigin.SetActive(true);
            SetLayerRecursively(avatarVisuals, LayerMask.NameToLayer("LocalBody"));
        }
        else
        {
            SetLayerRecursively(avatarVisuals, LayerMask.NameToLayer("RemoteBody"));
        }
    }

    void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform t in obj.transform) SetLayerRecursively(t.gameObject, layer);
    }
}