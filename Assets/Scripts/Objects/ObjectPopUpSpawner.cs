using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;
//VR 기기로 오브젝트 상호작용시 팝업 창 생성
public class ObjectPopUpSpawner : MonoBehaviour
{
    public GameObject popUpPrefab;
    public Transform popUpParent;
    public GameObject popUpObject;
    public Transform playerCamera;
    public float popUpDuration = 3.0f;
    public float popupDistance = 4.0f; // 팝업이 카메라로부터 얼마나 떨어져 있을지
    public float popUpDelay = 0.5f;
    public float popUpScale = 4.0f;
    public float popUpRotation = 0.0f;
    public float popUpPosition = 0.0f;
    public XRGrabInteractable interactable;
    private void Start()
    {
        if (popUpPrefab == null)
        {
            Debug.LogError("PopUp Prefab is not assigned!");
            return;
        }
        if (popUpParent == null)
        {
            Debug.LogError("PopUp Parent Transform is not assigned!");
            return;
        }
        playerCamera = Camera.main.transform;
        interactable = GetComponent<XRGrabInteractable>();
    }
    
    void OnEnable()
    {
        PadLockPassword.OnPasswordSuccess += OnPopUpEventEnd;
    }
    public void OnPopUpEvent()
    {
        
        Vector3 spawnPos = playerCamera.position + playerCamera.forward * popupDistance;
        if(PhotonNetwork.IsMasterClient)
        {
            popUpObject = PhotonNetwork.Instantiate(popUpPrefab.name, spawnPos, popUpParent.rotation);
        }
        else
        {
            popUpObject = Instantiate(popUpPrefab, spawnPos, popUpParent.rotation);
        }
        popUpObject.transform.LookAt(playerCamera); // 카메라 바라보게
        popUpObject.transform.localScale = Vector3.one * popUpScale;
        // 팝업 오브젝트에서 ObjectPopUpSpawner 컴포넌트 제거
        var spawner = popUpObject.GetComponent<ObjectPopUpSpawner>();
        if (spawner != null)
        {
            spawner.enabled = false;
        }
        print("OnPopUpEvent");
    }
    public void OnPopUpEventEnd()
    {
        StartCoroutine(WaitForOpenAndDestroy());
        
        print("OnPopUpEventEnd");
    }
    public void SyncPopUpObject()
    {
        
    }
    private void Update()
    {
        //SyncPopUpObject();
    }
    public IEnumerator WaitForOpenAndDestroy()
    {
        yield return new WaitForSeconds(2.0f);
        if(PhotonNetwork.InRoom)
        {
            PhotonNetwork.Destroy(popUpObject);
        }
        else
        {
            Destroy(popUpObject);
        }
    }
}
