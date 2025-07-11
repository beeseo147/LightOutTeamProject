using UnityEngine;
using System.Collections;

public class SimpleFadeOut : MonoBehaviour
{
    [Header("Fade Settings")]
    [SerializeField] private GameObject[] endFadePanels;
    [SerializeField] private float endFadeDuration = 2f;
    [SerializeField] private bool endAutoStart = true;
    
    private void Start()
    {
        if (endAutoStart)
        {
            StartFadeOut();
        }
    }
    
    public void StartFadeOut()
    {
        StartCoroutine(FadeOutCoroutine());
    }
    
    private IEnumerator FadeOutCoroutine()
    {
        // 모든 패널을 활성화하고 완전히 보이게 설정
        SetAllPanelsActive(true);
        SetAllPanelsAlpha(1f);
        
        float endElapsedTime = 0f;
        
        while (endElapsedTime < endFadeDuration)
        {
            endElapsedTime += Time.deltaTime;
            float endProgress = endElapsedTime / endFadeDuration;
            
            // 1에서 0으로 페이드아웃
            float endAlpha = 1f - endProgress;
            SetAllPanelsAlpha(endAlpha);
            
            yield return null;
        }
        
        // 완전히 투명하게 설정
        SetAllPanelsAlpha(0f);
        
        // 모든 패널 비활성화
        SetAllPanelsActive(false);
    }
    
    private void SetAllPanelsAlpha(float alpha)
    {
        if (endFadePanels == null) return;
        
        foreach (GameObject endPanel in endFadePanels)
        {
            if (endPanel != null)
            {
                UnityEngine.UI.Image endImage = endPanel.GetComponent<UnityEngine.UI.Image>();
                if (endImage != null)
                {
                    Color endColor = endImage.color;
                    endColor.a = alpha;
                    endImage.color = endColor;
                }
            }
        }
    }
    
    private void SetAllPanelsActive(bool active)
    {
        if (endFadePanels == null) return;
        
        foreach (GameObject endPanel in endFadePanels)
        {
            if (endPanel != null)
            {
                endPanel.SetActive(active);
            }
        }
    }
    
    // Inspector에서 테스트용 버튼
    [ContextMenu("Test Fade Out")]
    public void TestFadeOut()
    {
        StartFadeOut();
    }
} 