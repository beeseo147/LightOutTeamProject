using UnityEngine;

public class EndAudioManager : MonoBehaviour
{
    public static EndAudioManager Instance { get; private set; }
    
    [Header("Audio Sources")]
    [SerializeField] private AudioSource endEndingMusicSource;
    
    private void Awake()
    {
        // 싱글톤 패턴
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSources();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void InitializeAudioSources()
    {
        // Ending Music AudioSource 설정
        if (endEndingMusicSource == null)
        {
            endEndingMusicSource = gameObject.AddComponent<AudioSource>();
        }
        
        endEndingMusicSource.playOnAwake = false;
        endEndingMusicSource.loop = false; // 루프 안함
    }
    
    public void PlayEndingMusic(AudioClip endClip)
    {
        if (endEndingMusicSource != null && endClip != null)
        {
            endEndingMusicSource.clip = endClip;
            endEndingMusicSource.Play();
            
            // 음악이 끝나면 자동으로 AudioManager 파괴
            StartCoroutine(DestroyWhenMusicEnds());
        }
    }
    
    private System.Collections.IEnumerator DestroyWhenMusicEnds()
    {
        // 음악이 끝날 때까지 대기
        yield return new WaitForSeconds(endEndingMusicSource.clip.length);
        
        // 음악 재생 완료 후 AudioManager 파괴
        Destroy(gameObject);
    }
} 