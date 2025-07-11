using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingUI : MonoBehaviour
{
    public GameObject davidUI;
    public GameObject kevinUI;
    public GameObject timeoutUI; // 타임아웃 UI 추가

    void Start()
    {
        string winner = GameManager.Instance != null ? GameManager.Instance.whoWin : "None";

        // 예시: 닉네임에 따라 UI 오브젝트 활성화
        if (winner == "David")
        {
            davidUI.SetActive(true);
            kevinUI.SetActive(false);
            timeoutUI.SetActive(false);
        }
        else if (winner == "Kevin")
        {
            davidUI.SetActive(false);
            kevinUI.SetActive(true);
            timeoutUI.SetActive(false);
        }
        else if (winner == "Timeout") // 타임아웃 엔딩 추가
        {
            davidUI.SetActive(false);
            kevinUI.SetActive(false);
            timeoutUI.SetActive(true);
        }
        else
        {
            davidUI.SetActive(false);
            kevinUI.SetActive(false);
            timeoutUI.SetActive(false);
        }
    }
}
