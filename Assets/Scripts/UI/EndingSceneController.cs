using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndingSceneController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject endBlackPanel;
    
    [Header("Audio")]
    [SerializeField] private AudioSource endEndingMusic;
    
    [Header("Timing Settings")]
    [SerializeField] private float endFadeInDuration = 4f; // 검은 패널이 사라지는 시간
    [SerializeField] private float endMusicDelay = 4f; // 음악 재생까지 대기 시간
    [SerializeField] private float endSceneTransitionDelay = 4f; // 씬 전환까지 대기 시간
    [SerializeField] private float endFadeOutDuration = 3f; // 페이드아웃 시간
    
    private void Start()
    {
        InitializeEndingScene();
    }
    
    private void InitializeEndingScene()
    {
        // 초기 상태 설정 - 검은 패널이 완전히 보이도록
        endBlackPanel.SetActive(true);
        
        // 시퀀스 시작
        StartCoroutine(EndingSequence());
    }
    
    private IEnumerator EndingSequence()
    {
        // 1. 검은 패널이 4초간 서서히 사라짐
        yield return StartCoroutine(FadeOutBlackPanel());
        
        // 2. 4초 대기 후 음악 재생
        yield return new WaitForSeconds(endMusicDelay);
        PlayEndingMusic();
        
        // 3. 4초 대기 후 씬 전환
        yield return new WaitForSeconds(endSceneTransitionDelay);
        yield return StartCoroutine(FadeInAndLoadLobby());
    }
    
    private IEnumerator FadeOutBlackPanel()
    {
        float endElapsedTime = 0f;
        
        while (endElapsedTime < endFadeInDuration)
        {
            endElapsedTime += Time.deltaTime;
            float endProgress = endElapsedTime / endFadeInDuration;
            
            // 검은 패널이 서서히 투명해짐
            SetPanelAlpha(1f - endProgress);
            
            yield return null;
        }
        
        SetPanelAlpha(0f);
    }
    
    private void PlayEndingMusic()
    {
        if (endEndingMusic != null)
        {
            // AudioManager로 전환하여 DontDestroyOnLoad 설정
            EndAudioManager.Instance.PlayEndingMusic(endEndingMusic.clip);
        }
    }
    
    private IEnumerator FadeInAndLoadLobby()
    {
        // 검은 패널을 다시 나타나게 하면서 페이드아웃
        float endElapsedTime = 0f;
        
        while (endElapsedTime < endFadeOutDuration)
        {
            endElapsedTime += Time.deltaTime;
            float endProgress = endElapsedTime / endFadeOutDuration;
            
            SetPanelAlpha(endProgress);
            yield return null;
        }
        
        SetPanelAlpha(1f);
        
        // 페이드아웃 완료 후 잠시 대기
        yield return new WaitForSeconds(0.5f);
        
        // Lobby 씬으로 비동기 전환
        AsyncOperation endAsyncLoad = SceneManager.LoadSceneAsync("Lobby_Juok");
        endAsyncLoad.allowSceneActivation = false; // 씬 전환을 지연시킴
        
        // 로딩이 완료될 때까지 대기
        while (endAsyncLoad.progress < 0.9f)
        {
            yield return null;
        }
        
        // 씬 전환 허용
        endAsyncLoad.allowSceneActivation = true;
    }
    
    private void SetPanelAlpha(float alpha)
    {
        // Image 컴포넌트의 Color를 통해 투명도 조절
        if (endBlackPanel != null)
        {
            UnityEngine.UI.Image image = endBlackPanel.GetComponent<UnityEngine.UI.Image>();
            if (image != null)
            {
                Color color = image.color;
                color.a = alpha;
                image.color = color;
            }
        }
    }
} 