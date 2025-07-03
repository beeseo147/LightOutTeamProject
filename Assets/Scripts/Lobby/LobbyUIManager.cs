using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUIManager : MonoBehaviour
{
    [SerializeField] private LobbyView lobbyView;
    
    private LobbyModel lobbyModel;
    private LobbyController lobbyController;
    
    private void Start()
    {
        InitializeLobby();
    }
    
    private void InitializeLobby()
    {
        // MVC 컴포넌트 초기화
        lobbyModel = new LobbyModel();
        lobbyController = new LobbyController(lobbyModel, lobbyView);
        
        // 초기 데이터 설정
        lobbyModel.PlayerName = PlayerPrefs.GetString("PlayerName", "Player");
        lobbyModel.IsGameStartAvailable = true;
    }
    
    private void OnDestroy()
    {
        // 이벤트 해제
        
    }   
}
