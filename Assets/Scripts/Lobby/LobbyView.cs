using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LobbyView : MonoBehaviour
{
    [Header("UI References")]
    public Button startGameButton;
    public Button howToPlayButton;
    public Button settingsButton;
    public Button exitGameButton;
    
    [Header("Lobby UI")]
    public GameObject lobbyPanel;
    public Text waitingText;
    
    // UI 이벤트
    public event System.Action OnStartGameClicked;
    public event System.Action OnHowToPlayClicked;
    public event System.Action OnSettingsClicked;
    public event System.Action OnExitGameClicked;
    
    private void Awake()
    {
        // 버튼 이벤트 연결
        startGameButton.onClick.AddListener(() => OnStartGameClicked?.Invoke());
        howToPlayButton.onClick.AddListener(() => OnHowToPlayClicked?.Invoke());
        settingsButton.onClick.AddListener(() => OnSettingsClicked?.Invoke());
        exitGameButton.onClick.AddListener(() => OnExitGameClicked?.Invoke());
        lobbyPanel.SetActive(false);
    }
    
    // UI 업데이트 메서드들
    public void SetStartGameButtonInteractable(bool interactable)
    {
        print("SetStartGameButtonInteractable" + interactable);
        startGameButton.interactable = interactable;
        lobbyPanel.SetActive(!interactable);
    }
    
    public void PlayButtonClickSound()
    {
        // 사운드 재생 로직
    }

    // 로비 패널 활성화
    public void ShowLobby()
    {
        print("ShowLobby");
        lobbyPanel.SetActive(true);
    }

    // 로비 패널 비활성화
    public void HideLobby()
    {
        lobbyPanel.SetActive(false);
    }

    // 대기 텍스트 표시
    public void SetWaitingText(string text)
    {
        waitingText.text = text;
    }
}
