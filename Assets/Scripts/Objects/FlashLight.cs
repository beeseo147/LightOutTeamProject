using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FlashLight : GrabbableObject
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
        
        //트리거 이벤트 리스너 등록
        grab.activated.AddListener(OnTriggerPressed);
        grab.deactivated.AddListener(OnTriggerReleased);
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
    
    private void OnTriggerReleased(DeactivateEventArgs args)
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
}
