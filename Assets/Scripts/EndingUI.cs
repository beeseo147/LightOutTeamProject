using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingUI : MonoBehaviour
{
    public GameObject davidUI;
    public GameObject kevinUI;

    void Start()
    {
        string winner = GameManager.Instance != null ? GameManager.Instance.whoWin : "None";

        // 예시: 닉네임에 따라 UI 오브젝트 활성화
        if (winner == "David")
        {
            davidUI.SetActive(true);
            kevinUI.SetActive(false);
        }
        else if (winner == "Kevin")
        {
            davidUI.SetActive(false);
            kevinUI.SetActive(true);
        }
        else
        {
            davidUI.SetActive(false);
            kevinUI.SetActive(false);
        }
    }
}
