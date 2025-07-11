using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public string whoWin = "None";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환에도 유지
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
