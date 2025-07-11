using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class CheckPeople : MonoBehaviour
{
    public string objectKey; // "A", "B" 등
    public string eventHandlerObjectName = "DialogueFirst"; // Inspector에서 이름 지정 가능
    public ICheckPeopleEvent eventHandler;
    
    XRGrabInteractable grab;

    private void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnRelease);

        // 1. 동일 오브젝트에 붙어있는지 먼저 시도
        eventHandler = GetComponent<ICheckPeopleEvent>();

        // 2. 없으면 Object 이름으로 찾아서 할당
        if (eventHandler == null && !string.IsNullOrEmpty(eventHandlerObjectName))
        {
            GameObject obj = GameObject.Find(eventHandlerObjectName);
            if (obj != null)
            {
                eventHandler = obj.GetComponent<ICheckPeopleEvent>();
                print("eventHandler 할당 완료");
            }
        }
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        if (PhotonNetwork.InRoom)
            CheckPeopleManager.Instance.SetObjectSelected(objectKey, true);
    }

    void OnRelease(SelectExitEventArgs args)
    {
        if (PhotonNetwork.InRoom)
            CheckPeopleManager.Instance.SetObjectSelected(objectKey, false);
    }
}
