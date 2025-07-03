using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.Events;

public class PhotonNetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] UnityEvent joinedRoomEvent;

    void Start()
    {
        PhotonNetwork.NickName = "Player" + Random.Range(1000, 9999);
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        TryJoinOrCreateRoom();
        Debug.Log("마스터 서버 연결됨, 방 입장 시도");

    }

    public override void OnJoinedRoom()
    {
        Debug.Log("방 입장 성공");
        joinedRoomEvent.Invoke();    
    }

    void TryJoinOrCreateRoom()
    {
        var roomName = "MyRoom";
        var options = new RoomOptions { MaxPlayers = 2 };
        PhotonNetwork.JoinOrCreateRoom(roomName, options, TypedLobby.Default);
        Debug.Log("방 입장 또는 생성 시도");
    }
}
