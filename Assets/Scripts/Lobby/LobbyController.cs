using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class LobbyController : MonoBehaviour
{
    private LobbyModel model;
    private LobbyView view;
    
    public LobbyController(LobbyModel model, LobbyView view)
    {
        this.model = model;
        this.view = view;
        
        InitializeEvents();
    }
    
    private void InitializeEvents()
    {
        // View 이벤트 구독
        view.OnStartGameClicked += HandleStartGame;
        view.OnHowToPlayClicked += HandleHowToPlay;
        view.OnSettingsClicked += HandleSettings;
        view.OnExitGameClicked += HandleExitGame;
        view.OnHowToBackClicked += HandleHowToBack;
        view.OnSettingsBackClicked += HandleSettingsBack;
        
        // Model 이벤트 구독
        model.OnGameStartAvailable += view.SetStartGameButtonInteractable;
    }
    
    private void HandleStartGame()
    {
        // StartButton 클릭 시 로비 UI 활성화 및 대기 텍스트 표시
        view.ShowLobby();
        view.SetWaitingText("플레이어를 기다리는 중...");

        // 실제 네트워크 연결 및 룸 입장 로직은 LobbyManager 등에서 처리
        // 예: LobbyManager.Instance.Connect();
    }
    
    private void HandleHowToPlay()
    {
        print("HowToPlay 버튼 클릭");
        view.PlayButtonClickSound();
        view.HowToPanel.SetActive(true); // HowToPlay 패널 표시
        gameObject.GetComponent<TrackedDeviceGraphicRaycaster>().enabled = false;
    }
    
    private void HandleHowToBack()
    {
        view.HowToPanel.SetActive(false);
        gameObject.GetComponent<TrackedDeviceGraphicRaycaster>().enabled = true;
    }

    private void HandleSettings()
    {
        view.PlayButtonClickSound();
        // 설정 패널 표시
        view.SettingsPanel.SetActive(true);
        gameObject.GetComponent<TrackedDeviceGraphicRaycaster>().enabled = false;
    }

    private void HandleSettingsBack()
    {
        view.SettingsPanel.SetActive(false);
        gameObject.GetComponent<TrackedDeviceGraphicRaycaster>().enabled = true;
    }
    
    private void HandleExitGame()
    {
        view.PlayButtonClickSound();
        // 게임 종료 확인 팝업 표시
        ShowExitConfirmation();
    }
    
    private IEnumerator LoadGameScene()
    {
        yield return new WaitForSeconds(1f); // 로딩 시뮬레이션
    }
    
    private void ShowExitConfirmation()
    {
        // 확인 팝업 표시 로직
    }
}
