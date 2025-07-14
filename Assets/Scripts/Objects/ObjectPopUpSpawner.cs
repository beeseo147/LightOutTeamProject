using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit;
//VR 기기로 오브젝트 상호작용시 팝업 창 생성
public class ObjectPopUpSpawner : MonoBehaviourPun
{
    public GameObject popUpPrefab;
    public Transform popUpParent;
    public GameObject popUpObject;
    public Transform playerCamera;
    public float popUpDuration = 3.0f;
    public float popupDistance = 4.0f; // 팝업이 카메라로부터 얼마나 떨어져 있을지
    public float farDistance = 10.0f;
    public float popUpDelay = 0.5f;
    public float popUpScale = 1.0f;
    public float popUpRotation = 0.0f;
    public float popUpPosition = 0.0f;
    public XRGrabInteractable interactable;

    private bool bFarObject = false;
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
        
        interactable = GetComponent<XRGrabInteractable>();
    }
    
    void OnEnable()
    {
        PadLockPassword.OnPasswordSuccess += OnPopUpEventEnd;
    }
        // XR Simple Interactable에 직접 할당
    public void OnPopUpEvent(SelectEnterEventArgs args)
    {
        // 1. 상호작용한 플레이어의 XR Origin(혹은 카메라) Transform 얻기
        var interactor = args.interactorObject.transform;
        playerCamera = null;

        // XR Origin 구조에 따라 Camera 찾기
        // XR Origin → Camera Offset → Main Camera
        var xrOrigin = interactor.GetComponentInParent<XROrigin>();
        if (xrOrigin != null)
        {
            playerCamera = xrOrigin.Camera.transform;
        }
        else
        {
            // Fallback: interactor 바로 아래에 카메라가 있을 수도 있음
            playerCamera = interactor.GetComponentInChildren<Camera>()?.transform;
        }

        if (playerCamera == null)
        {
            Debug.LogError("플레이어 카메라를 찾을 수 없습니다!");
            return;
        }

        // 2. 해당 플레이어 카메라 기준으로 팝업 위치 계산
        Vector3 spawnPos = playerCamera.position + playerCamera.forward * popupDistance;

        if (PhotonNetwork.InRoom)
        {
            popUpObject = PhotonNetwork.Instantiate(popUpPrefab.name, spawnPos, Quaternion.identity);
        }
        else
        {
            popUpObject = Instantiate(popUpPrefab, spawnPos, Quaternion.identity);
        }

        popUpObject.transform.LookAt(playerCamera);
        popUpObject.transform.localScale = Vector3.one * popUpScale;

        
        var spawner = popUpObject.GetComponent<ObjectPopUpSpawner>();
        if (spawner != null)
        {
            spawner.enabled = false;
        }
        if(popUpObject != null)
        {
            this.popUpObject = popUpObject; // 반드시 멤버 변수에 할당!
        }
        else
        {
            print("popUpObject is null");
        }
    }
    public void OnPopUpEventEnd()
    {
        if (!isDestroying)
            StartCoroutine(WaitForOpenAndDestroy());
    }
    private bool isDestroying = false;

    private void Update()
    {
        if (!isDestroying && IsbFar())
        {
            StartCoroutine(WaitForOpenAndDestroy());
        }
    }
    public bool IsbFar()
    {
        if (popUpObject == null) return false;
        
        // 플레이어 카메라와 팝업 오브젝트 사이의 거리를 계산
        float distance = Vector3.Distance(playerCamera.position, popUpObject.transform.position);
        
        // 지정된 거리보다 멀리 떨어져 있으면 팝업 오브젝트를 비활성화
        if (distance > farDistance)
        {
            bFarObject = true;
            return true;
        }
        else
        {
            bFarObject = false;
            return false;
        }
    }


    public IEnumerator WaitForOpenAndDestroy()
    {
        isDestroying = true;
        yield return new WaitForSeconds(2.0f);

        if (popUpObject != null)
        {
            if (PhotonNetwork.InRoom)
            {
                Debug.Log("[Destroy] PhotonNetwork.Destroy 호출");
                if(bFarObject)
                {
                    PhotonNetwork.Destroy(popUpObject);
                }
                else
                {
                    PhotonNetwork.Destroy(popUpObject);
                    PhotonNetwork.Destroy(gameObject);
                }
            }
            else
            {
                Debug.Log("[Destroy] 일반 Destroy 호출");
                if(bFarObject)
                {
                    Destroy(popUpObject);
                }
                else
                {
                    Destroy(popUpObject);
                    Destroy(gameObject);
                }
            }
            popUpObject = null;
        }
        else
        {
            Debug.LogWarning("[Destroy] popUpObject가 이미 null입니다.");
        }
    }

    [PunRPC]
    public void RPC_OnPopUpEventEnd()
    {
        if (!isDestroying)
            StartCoroutine(WaitForOpenAndDestroy());
    }
}
