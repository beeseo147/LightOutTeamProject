using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public string whoWin = "None";

    // 타임아웃 관련 변수들
    [Header("Timeout Settings")]
    public bool isTimeoutActive = false;
    public float timeoutDuration = 60f;
    public float timeoutStartTime = 0f;
    public bool isTimeoutTriggered = false; // 타임아웃이 시작되었는지 확인

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

    // 타임아웃 시작
    public void StartTimeout()
    {
        if (!isTimeoutActive && !isTimeoutTriggered)
        {
            isTimeoutActive = true;
            isTimeoutTriggered = true;
            timeoutStartTime = Time.time;
            Debug.Log("타임아웃 시작: 60초 카운트다운");
        }
    }

    // 타임아웃 시간 확인
    public float GetRemainingTime()
    {
        if (!isTimeoutActive) return 0f;
        return Mathf.Max(0f, timeoutDuration - (Time.time - timeoutStartTime));
    }

    // 타임아웃 완료 확인
    public bool IsTimeoutComplete()
    {
        return isTimeoutActive && GetRemainingTime() <= 0f;
    }
}