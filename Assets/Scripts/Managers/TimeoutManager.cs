using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class TimeoutManager : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource clockAudioSource;
    [SerializeField] private AudioClip clockTickSound;
    
    [Header("UI")]
    [SerializeField] private Text countdownText; // 카운트다운 텍스트 추가
    [SerializeField] private GameObject timeoutUI; // 타임아웃 UI (옵션)
    
    [Header("Settings")]
    [SerializeField] private KeyCode timeoutTriggerKey = KeyCode.Alpha3; // 숫자 3 키
    
    private bool isClockPlaying = false;
    private int lastDisplayedSecond = -1; // 마지막으로 표시된 초 (중복 업데이트 방지)
    
    void Start()
    {
        // AudioSource가 없으면 자동으로 추가
        if (clockAudioSource == null)
        {
            clockAudioSource = gameObject.AddComponent<AudioSource>();
        }
        
        // 시계 소리 설정
        if (clockTickSound == null)
        {
            // Assets/Sound/ClockTick.ogg 파일을 찾아서 할당
            clockTickSound = Resources.Load<AudioClip>("ClockTick");
            if (clockTickSound == null)
            {
                // Resources 폴더에 없으면 직접 경로로 찾기
                clockTickSound = Resources.Load<AudioClip>("Sound/ClockTick");
            }
        }
        
        // AudioSource 설정
        if (clockAudioSource != null)
        {
            clockAudioSource.clip = clockTickSound;
            clockAudioSource.loop = true;
            clockAudioSource.playOnAwake = false;
            clockAudioSource.volume = 0.5f;
        }
        
        // 카운트다운 텍스트 초기화
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }
    }
    
    void Update()
    {
        // 숫자 3 키를 누르면 타임아웃 시작
        if (Input.GetKeyDown(timeoutTriggerKey))
        {
            if (!GameManager.Instance.isTimeoutTriggered)
            {
                StartTimeout();
            }
        }
        
        // 타임아웃이 활성화되어 있으면 처리
        if (GameManager.Instance.isTimeoutActive)
        {
            HandleTimeout();
        }
    }
    
    void StartTimeout()
    {
        GameManager.Instance.StartTimeout();
        StartClockSound();
        ShowCountdownText();
        Debug.Log("타임아웃 시스템 활성화됨 - 60초 카운트다운 시작");
    }
    
    void HandleTimeout()
    {
        float remainingTime = GameManager.Instance.GetRemainingTime();
        int currentSecond = Mathf.FloorToInt(remainingTime);
        
        // 카운트다운 텍스트 업데이트 (초가 바뀔 때만)
        if (currentSecond != lastDisplayedSecond && currentSecond >= 0)
        {
            UpdateCountdownText(currentSecond);
            lastDisplayedSecond = currentSecond;
        }
        
        // 남은 시간이 0 이하면 타임아웃 완료
        if (GameManager.Instance.IsTimeoutComplete())
        {
            CompleteTimeout();
        }
        
        // 디버그용: 남은 시간 출력 (10초마다)
        if (currentSecond % 10 == 0 && currentSecond > 0 && currentSecond < 60)
        {
            Debug.Log($"타임아웃 남은 시간: {currentSecond}초");
        }
    }
    
    void ShowCountdownText()
    {
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);
            UpdateCountdownText(60); // 60초부터 시작
        }
    }
    
    void UpdateCountdownText(int seconds)
    {
        if (countdownText != null)
        {
            countdownText.text = $"{seconds}초";
            
            // 시간에 따른 색상 변경 (선택사항)
            if (seconds <= 10)
            {
                countdownText.color = Color.red; // 10초 이하일 때 빨간색
            }
            else if (seconds <= 30)
            {
                countdownText.color = Color.yellow; // 30초 이하일 때 노란색
            }
            else
            {
                countdownText.color = Color.white; // 기본 흰색
            }
        }
    }
    
    void HideCountdownText()
    {
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }
    }
    
    void StartClockSound()
    {
        if (clockAudioSource != null && clockTickSound != null && !isClockPlaying)
        {
            clockAudioSource.Play();
            isClockPlaying = true;
            Debug.Log("시계 소리 재생 시작");
        }
    }
    
    // public으로 변경하여 외부에서 호출 가능
    public void StopClockSound()
    {
        if (clockAudioSource != null && isClockPlaying)
        {
            clockAudioSource.Stop();
            isClockPlaying = false;
            Debug.Log("시계 소리 재생 중지");
        }
        
        // 카운트다운 텍스트 숨기기
        HideCountdownText();
        lastDisplayedSecond = -1; // 리셋
    }
    
    void CompleteTimeout()
    {
        StopClockSound();
        GameManager.Instance.isTimeoutActive = false;
        
        // 타임아웃으로 인한 엔딩 처리
        GameManager.Instance.whoWin = "Timeout"; // 타임아웃 표시
        
        Debug.Log("타임아웃 완료 - EndingScene으로 전환");
        
        // EndingScene으로 전환
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LoadLevel("EndingScene");
        }
        else
        {
            PhotonNetwork.LoadLevel("EndingScene");
        }
    }
} 