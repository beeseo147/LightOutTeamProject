using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
using Photon.Realtime;

public class CheckPeopleObject : MonoBehaviourPunCallbacks
{
    public string objectKey; // "A" 또는 "B"
    private XRBaseInteractable interactable;

    void Start()
    {
        interactable = GetComponent<XRBaseInteractable>();
        interactable.selectEntered.AddListener(OnSelectEntered);
        interactable.selectExited.AddListener(OnSelectExited);
    }

    void OnSelectEntered(SelectEnterEventArgs args)
    {
        // 잡았음을 네트워크에 알림
        SetGrabState(true);
    }

    void OnSelectExited(SelectExitEventArgs args)
    {
        // 놓았음을 네트워크에 알림
        SetGrabState(false);
    }

    void SetGrabState(bool isGrabbed)
    {
        // Custom Property로 상태 공유
        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();
        props[objectKey + "_Grabbed"] = isGrabbed;
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);
    }
}
