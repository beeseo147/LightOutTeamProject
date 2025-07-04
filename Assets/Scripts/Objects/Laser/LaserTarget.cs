using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class LaserTarget : MonoBehaviour
{
    [Header("Feedback")]
    [SerializeField] Color hitColor = Color.cyan;
    [SerializeField] ParticleSystem hitParticle;
    [SerializeField] AudioSource hitSound;

    [SerializeField] float maxCharge = 1f;
    float curCharge = 0f;

    // ���� �Ŵ����� �о� �� �� �ֵ���
    public bool IsHit => isHit;
    public bool IsCleared => isCleared;
    bool isCleared = false;
    bool isHit = false;
    Color originalColor;
    Renderer rend;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
    }

    /// Laser -> Target
    public void Activate(Vector3 hitPos)
    {
        isHit = true;
        rend.material.color = hitColor;

        if (isCleared) 
            return;

        if (hitParticle) 
        { 
            hitParticle.transform.position = hitPos;
            hitParticle.Play(); 
        }
        if (hitSound)
            hitSound.Play();
    }

    /// Laser�� �� �̻� ���� ���� �� ȣ��(����)
    public void Deactivate()
    {
        if (!isHit)
            return;

        isHit = false;
        if (!isCleared)
            rend.material.color = originalColor;

        if (hitParticle)
            hitParticle.Stop();
    }

    IEnumerator CoCharging()
    {
        while (curCharge < maxCharge)
        {
            curCharge += Time.deltaTime;
        }

        curCharge = 0f;

        yield return null;
    }
}