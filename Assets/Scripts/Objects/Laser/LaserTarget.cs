using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using JetBrains.Annotations;
using UnityEngine.UIElements;
using TMPro;
using UnityEngine.Events;

[RequireComponent(typeof(Renderer))]
public class LaserTarget : MonoBehaviourPun
{
    [Header("Hit")]
    [SerializeField] Color hitColor = Color.cyan;
    [SerializeField] AudioClip hitSound;

    [Header("Clear")]
    [SerializeField] UnityEvent clearEvent;
    [SerializeField] Color clearColor = Color.red;
    [SerializeField] ParticleSystem clearParticle;
    [SerializeField] AudioClip clearSound;

    [Header("MaxTime")]
    [SerializeField] float maxCharge = 2f;
    public float curCharge = 0f;

    bool isCleared = false;
    bool isHitThisFrame = false;
    bool isCharging = false;
    Color originalColor;

    TextMeshPro hintNumText;
    AudioSource audioSource;
    Renderer rend;
    //Material rendMat;

    void Awake()
    {
        hintNumText = GetComponentInChildren<TextMeshPro>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource)
        {
            audioSource.loop = false;
            audioSource.playOnAwake = false;
        }

        rend = GetComponent<Renderer>();
        if (rend)
        {
            originalColor = rend.material.color;
        }
    }

    /// Laser -> Target
    public void Activate()
    {
        if (isCleared)
            return;

        isHitThisFrame = true;
    }

    public void DeActivate()
    {
        isHitThisFrame = false;
    }

    void Update()
    {
        if (isCleared)
            return;

        if (isHitThisFrame)
        {
            if (!isCharging)
            {
                // First Start
                rend.material.color = hitColor;

                photonView.RPC("PlayHitSound", RpcTarget.All);
                //PlaySound(hitSound);
                isCharging = true;
            }

            curCharge += Time.deltaTime;

            if (curCharge >= maxCharge)
            {
                Clear();
            }
        }
        else
        {
            if (isCharging)
            {
                // Stop Charging
                rend.material.color = originalColor;
                curCharge = 0f;
                isCharging = false;
            }
        }
    }

    private void Clear()
    {
        isCleared = true;
        rend.material.color = clearColor;

        //if (clearParticle)
        //    clearParticle.Play();
        //photonView.RPC("PlaySound", RpcTarget.All, "clearSound");
        clearEvent.Invoke();
        //PlaySound(clearSound)
    }

    //private void PlaySound(string clipName)
    [PunRPC]
    private void PlayHitSound()
    {
        //AudioClip clip = Resources.Load<AudioClip>(clipName);
        if (audioSource.isPlaying)
            return;

        if (hitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hitSound);
        }
        else
        {
            Debug.LogWarning($"AudioClip '{hitSound.name}' not found in Resources.");
        }
    }

    public void CallPlayClearSound()
    {
        photonView.RPC("PlayClearSound", RpcTarget.All);
    }

    [PunRPC]
    private void PlayClearSound()
    {
        //AudioClip clip = Resources.Load<AudioClip>(clipName);
        if (audioSource.isPlaying)
            return;

        if (clearSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clearSound);
        }
        else
        {
            Debug.LogWarning($"AudioClip '{clearSound.name}' not found in Resources.");
        }
    }
}