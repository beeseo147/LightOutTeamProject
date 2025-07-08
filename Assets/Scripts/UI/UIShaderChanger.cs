using UnityEngine;
using UnityEngine.UI;

public class UIShaderChanger : MonoBehaviour
{
    [Header("UI Unlit Material")]
    [SerializeField] private Material uiUnlitMaterial;
    
    void Start()
    {
        if (uiUnlitMaterial != null)
        {
            ApplyUIMaterial();
        }
    }
    
    [ContextMenu("Apply UI Material")]
    public void ApplyUIMaterial()
    {
        if (uiUnlitMaterial == null)
        {
            Debug.LogError("UI Unlit Material을 Inspector에서 할당해주세요!");
            return;
        }
        
        // Image 컴포넌트 처리
        Image image = GetComponent<Image>();
        if (image != null)
        {
            image.material = uiUnlitMaterial;
        }
        
        // RawImage 컴포넌트 처리
        RawImage rawImage = GetComponent<RawImage>();
        if (rawImage != null)
        {
            rawImage.material = uiUnlitMaterial;
        }
        
        // Text 컴포넌트 처리
        Text text = GetComponent<Text>();
        if (text != null)
        {
            text.material = uiUnlitMaterial;
        }
    }
} 