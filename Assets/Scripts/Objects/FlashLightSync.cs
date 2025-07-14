using UnityEngine;
using Photon.Pun;

/// <summary>
/// 손전등의 라이트 상태를 네트워크로 동기화하는 컴포넌트
/// </summary>
public class FlashLightSync : MonoBehaviourPun, IPunObservable
{
    [Header("Flashlight Components")]
    [SerializeField] private Light flashlightLight;   // 손전등 라이트 컴포넌트
    [SerializeField] private AudioSource switchSound; // 스위치 소리 오브젝트
    
    private bool isLightOn = false;                   // 손전등 켜짐 상태

    private void Start()
    {
        // 라이트 초기화
        if(flashlightLight != null)
        {
            flashlightLight.enabled = false;
        }
    }

    public void SetLightState(bool state)
    {
        isLightOn = state;
        if(flashlightLight != null)
        {
            flashlightLight.enabled = isLightOn;
        }
    }

    public void ToggleLight()
    {
        SetLightState(!isLightOn);
        
        if(switchSound != null)
        {
            switchSound.Play();
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 내가 소유한 손전등일 때: 라이트 상태를 네트워크로 전송
            stream.SendNext(isLightOn);
        }
        else
        {
            // 다른 플레이어의 손전등일 때: 네트워크에서 라이트 상태를 받아서 적용
            bool networkLightState = (bool)stream.ReceiveNext();
            
            // 네트워크 상태와 현재 상태가 다르면 업데이트
            if (networkLightState != isLightOn)
            {
                SetLightState(networkLightState);
            }
        }
    }
} 