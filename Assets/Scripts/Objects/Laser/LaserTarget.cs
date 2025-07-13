using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using JetBrains.Annotations;

[RequireComponent(typeof(Renderer))]
public class LaserTarget : MonoBehaviourPun
{
    [Header("Feedback")]
    [SerializeField] Color hitColor = Color.cyan;
    [SerializeField] Color clearColor = Color.red;
    [SerializeField] ParticleSystem clearParticle;
    [SerializeField] AudioClip hitSound;
    [SerializeField] AudioClip clearSound;

    [SerializeField] float maxCharge = 1f;
    public float curCharge = 0f;
    bool isHitThisFrame = false;
    bool isCurrentlyLit = false;
    AudioSource audioSource;

    //public bool IsHit => isHit;
    public bool IsCleared => isCleared;
    bool isCleared = false;
    //bool isHit = false;
    Color originalColor;
    Renderer rend;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
    }

    /// Laser -> Target
    public void Activate()
    {
        //if (0 == curCharge)
        if (isCleared)
            return;

        rend.material.color = hitColor;
        isCurrentlyLit = true;
            //curCharge += Time.deltaTime;
            //isHitThisFrame = true;
    }

    public void DeActivate()
    {
        rend.material.color = originalColor;
        isCurrentlyLit = false;
    }

    public void Clear()
    {
        isCleared = true;
        rend.material.color = clearColor;
        isCurrentlyLit = false;
    }


    private void FixedUpdate()
    {
        //CheckCurrentOn();
        //isHitThisFrame = false;
    }

    //private void LateUpdate()
    //{
    //    isHitThisFrame = false;
    //}

    void CheckCurrentOn()
    {
        // Charging .. 
        if (1 <= curCharge)
        {
            if (!isCurrentlyLit)
            {
                rend.material.color = hitColor;
                isCurrentlyLit = true;

                //if (hitSound)
                //{
                //    audioSource.clip = hitSound;
                //    audioSource.Play();
                //}
            }
            //curCharge += Time.deltaTime;

            if (curCharge >= maxCharge)
            {
                Debug.Log("PUZZLE CLEARED!");
                isCleared = true;

                /* Effect */
                //if (clearSound)
                //{
                //    audioSource.clip = clearSound;
                //    audioSource.Play();
                //}

                //if (clearParticle)
                //    clearParticle.Play();
            }
        }
        // Finish Charging
        else
        {
            if (isCurrentlyLit)
            {
                rend.material.color = originalColor;
                curCharge = 0f;
                isCurrentlyLit = false;
            }
        }
    }

}