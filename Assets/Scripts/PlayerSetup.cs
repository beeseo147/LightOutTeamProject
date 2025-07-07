using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class PlayerSetup : MonoBehaviourPun
{
    [SerializeField] GameObject xrOrigin;        // XR Origin �Ѹ�
    [SerializeField] GameObject avatarVisuals;   // �� ��ü(�޽� ����)
    [SerializeField] GameObject leftHandModel;
    [SerializeField] GameObject rightHandModel;

    void Awake()
    {
        if (photonView.IsMine)
        {
            avatarVisuals.SetActive(false);
            var myCam = xrOrigin.GetComponentInChildren<Camera>(true);
            if (myCam != null)
                myCam.gameObject.SetActive(true);
            //xrOrigin.SetActive(true);
            //SetLayerRecursively(avatarVisuals, LayerMask.NameToLayer("LocalBody"));
        }
        else
        {
            leftHandModel.SetActive(false);
            rightHandModel.SetActive(false);
            avatarVisuals.SetActive(true);
            GetComponentInChildren<LocomotionSystem>().enabled = false;
            GetComponentInChildren<InputActionManager>().enabled = false;
            var cam = xrOrigin.GetComponentInChildren<Camera>(true);
            if (cam != null)
                cam.gameObject.SetActive(false);

            var handAnimators = GetComponentsInChildren<HandAnimationController>(true);
            foreach (var h in handAnimators)
                h.enabled = false;

            var trackedPoseDrivers = xrOrigin.GetComponentsInChildren<UnityEngine.SpatialTracking.TrackedPoseDriver>(true);
            foreach (var tpd in trackedPoseDrivers)
                tpd.enabled = false;
            //xrOrigin.SetActive(false);
            //SetLayerRecursively(avatarVisuals, LayerMask.NameToLayer("RemoteBody"));
        }
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
