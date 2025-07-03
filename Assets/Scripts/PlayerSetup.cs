using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerSetup : MonoBehaviourPun
{
    [SerializeField] GameObject xrOrigin;        // XR Origin »Ñ¸®
    [SerializeField] GameObject avatarVisuals;   // ¸ö ÀüÃ¼(¸Þ½Ã Æ÷ÇÔ)

    void Awake()
    {
        if(photonView.IsMine)
        {
            xrOrigin.SetActive(true);
            SetLayerRecursively(avatarVisuals, LayerMask.NameToLayer("LocalBody"));
        }
        else
        {
            xrOrigin.SetActive(false);
            SetLayerRecursively(avatarVisuals, LayerMask.NameToLayer("RemoteBody"));
        }
    }

    void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach(Transform t in obj.transform) SetLayerRecursively(t.gameObject, layer);
    }
}
