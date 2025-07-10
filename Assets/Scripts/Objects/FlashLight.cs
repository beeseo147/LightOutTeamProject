using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;   

public class FlashLight : GrabbableObject, IPunObservable
{
    [Header("Flashlight Settings")]
    [SerializeField] private Light flashlightLight;   // 손전등 라이트 컴포넌트
    [SerializeField] private AudioSource switchSound; // 스위치 소리 오브젝트

    private bool isLightOn = false;                   // 손전등 켜짐 상태
    private bool isGrabbed = false;                   // 손전등 잡힘 상태

    private void Start()
    {
        // 라이트 초기화
        if(flashlightLight != null)
        {
            flashlightLight.enabled = false;
        }
        
        // 트리거 이벤트 리스너 등록 (누를 때만)
        grab.activated.AddListener(OnTriggerPressed);
    }

    private void Update()
    {
        // 그랩 상태 확인
        isGrabbed = grab.isSelected;
    }

    private void OnTriggerPressed(ActivateEventArgs args)
    {
        if(isGrabbed)
        {
            ToggleLight();
        }
    }
    
    private void ToggleLight()
    {
        isLightOn = !isLightOn;
        flashlightLight.enabled = isLightOn;

        if(switchSound != null)
        {
            switchSound.Play();
        }
    }

    // 네트워크 동기화를 위한 OnPhotonSerializeView 구현
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 내가 소유한 손전등일 때: 상태를 네트워크로 전송
            stream.SendNext(isLightOn);
        }
        else
        {
            // 다른 플레이어의 손전등일 때: 네트워크에서 상태를 받아서 적용
            bool networkLightState = (bool)stream.ReceiveNext();
            
            // 네트워크 상태와 현재 상태가 다르면 업데이트
            if (networkLightState != isLightOn)
            {
                isLightOn = networkLightState;
                flashlightLight.enabled = isLightOn;
            }
        }
    }
}
