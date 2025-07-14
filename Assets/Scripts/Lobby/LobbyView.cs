using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using TMPro;
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
    public GameObject HowToPanel; // HowToPlay 패널
    public GameObject SettingsPanel; // Settings 패널

    // UI 이벤트
    public event System.Action OnStartGameClicked;
    public event System.Action OnHowToPlayClicked;
    public event System.Action OnSettingsClicked;
    public event System.Action OnExitGameClicked;
    public event System.Action OnHowToBackClicked;
    public event System.Action OnSettingsBackClicked;
    
    private void Awake()
    {
        // 버튼 이벤트 연결
        startGameButton.onClick.AddListener(() => OnStartGameClicked?.Invoke());
        howToPlayButton.onClick.AddListener(() => OnHowToPlayClicked?.Invoke());
        settingsButton.onClick.AddListener(() => OnSettingsClicked?.Invoke());
        exitGameButton.onClick.AddListener(() => OnExitGameClicked?.Invoke());
        lobbyPanel.SetActive(false);

        // HowToPanel의 BackButton 연결
        var howToBackBtn = HowToPanel.transform.Find("BackButton")?.GetComponent<Button>();
        if (howToBackBtn != null)
            howToBackBtn.onClick.AddListener(() => OnHowToBackClicked?.Invoke());

        // SettingsPanel의 BackButton 연결
        var settingsBackBtn = SettingsPanel.transform.Find("BackButton")?.GetComponent<Button>();
        if (settingsBackBtn != null)
            settingsBackBtn.onClick.AddListener(() => OnSettingsBackClicked?.Invoke());
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

    // 예시: 메시지 표시용 텍스트
    [SerializeField] private Text messageText;
    // 메시지 표시
    public void ShowMessage(string message)
    {
        if (messageText != null)
            messageText.text = message;
        else
            Debug.Log(message); // 텍스트가 없으면 콘솔에 출력
    }

    // 대기 텍스트 표시 (이미 있을 수 있음)
    public void SetWaitingText(string text)
    {
        if (waitingText != null)
            waitingText.text = text;
    }
}
