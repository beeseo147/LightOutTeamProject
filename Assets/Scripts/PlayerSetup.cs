using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerSetup : MonoBehaviourPun
{
    [SerializeField] GameObject xrOrigin;        // XR Origin �Ѹ�
    [SerializeField] GameObject avatarVisuals;   // �� ��ü(�޽� ����)
    [SerializeField] GameObject leftHandModel;
    [SerializeField] GameObject rightHandModel;

    void Awake()
    {
        //if (photonView.IsMine)
        //{
        //    avatarVisuals.SetActive(false);
        //    //SetLayerRecursively(avatarVisuals, LayerMask.NameToLayer("LocalBody"));
        //}
        //else
        //{
        //    leftHandModel.SetActive(false);
        //    rightHandModel.SetActive(false);
        //    //xrOrigin.SetActive(false);
        //    //SetLayerRecursively(avatarVisuals, LayerMask.NameToLayer("RemoteBody"));
        //}
        if (!photonView.IsMine)
            GetComponentInChildren<LocomotionSystem>().enabled = false;
    }

    private void Update()
    {
        if(!photonView.IsMine)
        {

        }
    }

    void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach(Transform t in obj.transform) SetLayerRecursively(t.gameObject, layer);
    }
}
