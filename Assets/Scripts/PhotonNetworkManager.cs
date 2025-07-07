using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.Events;
using JetBrains.Annotations;
using UnityEngine.XR.Interaction.Toolkit;

public class PhotonNetworkManager : MonoBehaviourPunCallbacks
{
    [Header("Network Settings")]
    [SerializeField] UnityEvent joinedRoomEvent;
    [SerializeField] GameObject ServerPlayer;
    [SerializeField] GameObject ClientPlayer;
    [SerializeField] Transform spawnPoint1;
    [SerializeField] Transform spawnPoint2;

    GameObject myPlayer;
    Transform  spawnPoint;

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
        Debug.Log("방 입장 완료");
        joinedRoomEvent.Invoke();
        
        // Resources 폴더의 Player 프리팹을 원점에 스폰
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        myPlayer    = PhotonNetwork.IsMasterClient ? ServerPlayer : ClientPlayer;
        spawnPoint  = PhotonNetwork.IsMasterClient ? spawnPoint1  : spawnPoint2;

        Debug.Log($"spawnPlayer : {myPlayer.name}, spawnPoint : {spawnPoint}");

        // photon : different Position SPawn X,
        // -> Spawn all at once, then move them separately
        var playerObj = PhotonNetwork.Instantiate(myPlayer.name, Vector3.zero, Quaternion.identity);
        //PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);

        if (playerObj.GetComponent<PhotonView>().IsMine)
        {
            playerObj.transform.position = spawnPoint.position;
            playerObj.transform.rotation = spawnPoint.rotation;
        }
    }

    void TryJoinOrCreateRoom()
    {
        var roomName = "MyRoom";
        var options = new RoomOptions { MaxPlayers = 2 };
        PhotonNetwork.JoinOrCreateRoom(roomName, options, TypedLobby.Default);
        Debug.Log("방 입장 또는 생성 시도");
    }
}
