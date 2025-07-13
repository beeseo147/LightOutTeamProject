using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        view.PlayButtonClickSound();
    }
    
    private void HandleSettings()
    {
        view.PlayButtonClickSound();
        // 설정 패널 표시
        //SettingsManager.Instance.ShowSettings();
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
