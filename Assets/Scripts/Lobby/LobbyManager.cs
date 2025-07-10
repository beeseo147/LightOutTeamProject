using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using ExitGames.Client.Photon;
//Photon Unity Networking
//포톤을 사용하여 멀티플레이를 구현하기 위한 클래스
//Player 가 접속시 방을 생성하고 이미 존재하는 방에 접속하는 기능을 구현

public class LobbyManager : MonoBehaviourPunCallbacks
{
    //게임 버전
    private const string gameVersion = "1.0";
    //이름 두가지중 하나를 선택해서 사용
    //본인이 Master 라면 David 라고 설정 그외 Player Kevin 이라고 설정
    public string userName;
    public LobbyView lobbyView; // LobbyView 스크립트 참조

    //게임 실행시 최초 실행되는 함수
    void Start()
    {
        // 접속에 필요한 정보(게임 버전) 설정
        PhotonNetwork.GameVersion = gameVersion;
        // 설정한 정보를 가지고 마스터 서버 접속 시도
        PhotonNetwork.ConnectUsingSettings();

        // LobbyView의 StartGameButton 클릭 시 Connect() 실행
        if (lobbyView != null)
            lobbyView.OnStartGameClicked += Connect;
    }
    //마스터 서버에 접속 완료 시 자동 실행
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
    }
    //로비에 접속 완료 시 자동 실행
    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
    }
    //마스터 서버에서 접속 종료 시 자동 실행
    public override void OnDisconnected(DisconnectCause cause)
    {
        // 접속 정보 표시
        print("오프라인 : 마스터 서버와 연결되지 않음\n접속 재시도 중...");
        // 마스터 서버로의 재접속 시도
        PhotonNetwork.ConnectUsingSettings();
        lobbyView.SetStartGameButtonInteractable(false);
    }

    // 룸 접속 시도 버튼 클릭 시 호출되는 함수 예시) Button 클릭시
    public void Connect()
    {
        lobbyView.SetStartGameButtonInteractable(false); // 중복 클릭 방지

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    // (빈 방이 없어)랜덤 룸 참가에 실패한 경우 자동 실행
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // 최대 2명을 수용 가능한 빈방을 생성
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    // 룸에 참가 완료된 경우 자동 실행
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.NickName = "David";
        else
            PhotonNetwork.NickName = "Kevin";

        // 로비 텍스트 갱신
        UpdateLobbyWaitingText();
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            PhotonNetwork.LoadLevel("MainScene");
        }
    }

    // 방에 새로운 플레이어가 들어올 때마다 호출됨
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // 닉네임 자동 할당 (예: Kevin, David 등)
        if (newPlayer.IsMasterClient)
            newPlayer.NickName = "David";
        else
            newPlayer.NickName = "Kevin";

        // UI 갱신
        UpdateLobbyWaitingText();

        // 알림 메시지
        lobbyView?.ShowMessage($"{newPlayer.NickName} 님이 입장했습니다.");

        // 2명 모이면 자동 시작
       if (PhotonNetwork.CurrentRoom.PlayerCount == 2 && PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel("MainScene");
    }

    // (선택) 방에 플레이어가 나갈 때도 처리하고 싶으면
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdateLobbyWaitingText();
        lobbyView?.ShowMessage($"{otherPlayer.NickName} 님이 퇴장했습니다.");
    }

    private void UpdateLobbyWaitingText()
    {
        if (lobbyView != null)
        {
            int current = PhotonNetwork.CurrentRoom.PlayerCount;
            int max = PhotonNetwork.CurrentRoom.MaxPlayers;
            if (current < max)
                lobbyView.SetWaitingText($"플레이어를 기다리는 중... ({current}/{max})");
            else
                lobbyView.SetWaitingText("게임을 시작합니다!");
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        lobbyView?.ShowMessage($"{newMasterClient.NickName} 님이 새로운 마스터가 되었습니다.");
        UpdateLobbyWaitingText();
    }


    public override void OnCreatedRoom()
    {
        lobbyView?.ShowMessage("방이 성공적으로 생성되었습니다.");
        UpdateLobbyWaitingText();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        lobbyView?.ShowMessage($"방 생성 실패: {message}");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        lobbyView?.ShowMessage($"방 참가 실패: {message}");
    }

    public override void OnLeftRoom()
    {
        lobbyView?.ShowMessage("방에서 나갔습니다.");
        // 로비 UI로 복귀 등 추가 처리
    }

    public override void OnLeftLobby()
    {
        lobbyView?.ShowMessage("로비에서 나갔습니다.");
    }

    public override void OnConnected()
    {
        lobbyView?.ShowMessage("Photon 서버에 연결되었습니다.");
    }
}
