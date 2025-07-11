using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//BoxCollider 에 접근 하면 보내버림
public class EndingBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // 타임아웃이 활성화되어 있으면 타임아웃 취소
        if (GameManager.Instance != null && GameManager.Instance.isTimeoutActive)
        {
            // 타임아웃 취소
            GameManager.Instance.isTimeoutActive = false;
            GameManager.Instance.isTimeoutTriggered = false;
            
            // 시계 소리 중지
            var timeoutManager = FindObjectOfType<TimeoutManager>();
            if (timeoutManager != null)
            {
                timeoutManager.StopClockSound();
            }
            
            Debug.Log("타임아웃 중 탈출 성공 - 타임아웃 취소됨");
        }
        
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
