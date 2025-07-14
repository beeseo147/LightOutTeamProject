using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun; // 이 줄을 추가

public class FlashLight : GrabbableObject
{
    [Header("Flashlight Settings")]
    [SerializeField] private FlashLightSync lightSync; // 라이트 동기화 컴포넌트

    private bool isGrabbed = false;                    // 손전등 잡힘 상태

    private void Start()
    {
        // 트리거 이벤트 리스너 등록 (누를 때만)
        grab.activated.AddListener(OnTriggerPressed);
        
        // 그랩 이벤트 리스너 등록 (소유권 전환을 위해)
        grab.selectEntered.AddListener(OnFlashlightGrabbed);
        grab.selectExited.AddListener(OnFlashlightReleased);
        
        // 디버그 로그 추가
        Debug.Log($"[FlashLight] {name}: 초기화 - IsMine: {photonView.IsMine}, Owner: {photonView.Owner}");
    }

    private void Update()
    {
        // 그랩 상태 확인
        bool wasGrabbed = isGrabbed;
        isGrabbed = grab.isSelected;
        
        // 그랩 상태가 변경되면 로그 출력
        if (wasGrabbed != isGrabbed)
        {
            Debug.Log($"[FlashLight] {name}: 그랩 상태 변경 - IsGrabbed: {isGrabbed}, IsMine: {photonView.IsMine}, Owner: {photonView.Owner}");
        }
    }

    private void OnTriggerPressed(ActivateEventArgs args)
    {
        if(isGrabbed)
        {
            Debug.Log($"[FlashLight] {name}: 트리거 눌림 - IsMine: {photonView.IsMine}");
            lightSync.ToggleLight();
        }
    }

    // 손전등을 잡았을 때 호출되는 메서드
    private void OnFlashlightGrabbed(SelectEnterEventArgs args)
    {
        // 소유권 전환
        photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
        
        Debug.Log($"[FlashLight] {name}: 소유권 전환 시도 - IsMine: {photonView.IsMine}, Owner: {photonView.Owner}");
    }

    // 손전등을 놓았을 때 호출되는 메서드
    private void OnFlashlightReleased(SelectExitEventArgs args)
    {
        Debug.Log($"[FlashLight] {name}: 손전등 놓음 - IsMine: {photonView.IsMine}, Owner: {photonView.Owner}");
    }
}
