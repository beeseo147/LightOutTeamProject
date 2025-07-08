using UnityEngine;

[ExecuteAlways] // 에디터 모드에서도 실행, 테스트 편리
public class RevealObject : MonoBehaviour
{
    [SerializeField] Light spotLight;
    private Material m_Mat;

    private void Start()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
            m_Mat = GetComponent<Renderer>().sharedMaterial; // ExecuteAlways 때문에 Start()가 에디터에서도 호출,
                                                             // 에디터에서는 sharedMaterial(여러 객체가 공유하는 원본 머티리얼)을 써야 머티리얼 인스턴스 누수 경고 없이 안전하게 테스트 가능
        else
            m_Mat = GetComponent<Renderer>().material;
#else
    m_Mat = GetComponent<Renderer>().material;
#endif
    }

    private void Update()
    {
        if (spotLight != null && spotLight.enabled && spotLight.intensity > 0f)
        {
            m_Mat.SetVector("_LightPos", spotLight.transform.position);
            m_Mat.SetVector("_LightDir", -spotLight.transform.forward);
        }
        else
        {
            // reveal이 절대 안 되도록
            m_Mat.SetVector("_LightPos", new Vector3(99999, 99999, 99999));
            m_Mat.SetVector("_LightDir", Vector3.forward);
        }
    }
}