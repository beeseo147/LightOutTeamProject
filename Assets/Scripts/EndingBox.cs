using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//BoxCollider 에 접근 하면 보내버림
public class EndingBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(PhotonNetwork.InRoom)
        {
            // PhotonView가 있는 오브젝트만 처리
            var pv = other.GetComponent<PhotonView>();
            if (pv != null && pv.Owner != null)
            {
                GameManager.Instance.whoWin = pv.Owner.NickName;
            }
            PhotonNetwork.LoadLevel("EndingScene");
        }
        else
        {
            GameManager.Instance.whoWin = "Kevin";
            PhotonNetwork.LoadLevel("EndingScene");
        }
    }
}
