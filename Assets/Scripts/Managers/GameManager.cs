using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] string playerPrefabName = "Player";   // Resources/Player.prefab
    [SerializeField] Vector3 spawnPos = Vector3.zero;

    public override void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate(playerPrefabName, spawnPos, Quaternion.identity);
    }
}
