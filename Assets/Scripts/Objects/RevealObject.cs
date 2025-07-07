using UnityEngine;

[ExecuteAlways] // 에디터 모드에서도 실행되어 테스트가 간편
public class RevealObject : MonoBehaviour
{
    [SerializeField] Light spotLight;

    private Material m_Mat;

    private void Start()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
            m_Mat = GetComponent<Renderer>().sharedMaterial;
        else
            m_Mat = GetComponent<Renderer>().material;
#else
    m_Mat = GetComponent<Renderer>().material;
#endif
    }

    private void Update()
    {
        m_Mat.SetVector("_MyLightPosition", spotLight.transform.position);
        m_Mat.SetVector("_MyLightDirection", -spotLight.transform.forward);
    }
}