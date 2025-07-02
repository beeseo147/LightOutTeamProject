using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animControl : MonoBehaviour {

    // Talk to the Animator

    Animator m_animator;
    

    // Use this for initialization
    void Start () {
        m_animator = GetComponent<Animator>();
       
    }
	
	// Update is called once per frame
	void Update () {

        // quit
        {
            if (Input.GetKey("escape"))
            {
                Application.Quit();
            }
        }


        // anim

        bool bNeutral = Input.GetKey("0"); // 
        m_animator.SetBool("bNeutral", bNeutral);

        bool bComeHere = Input.GetKey("1"); // 
        m_animator.SetBool("bComeHere", bComeHere);

        bool bThumbsy= Input.GetKey("2"); // 
        m_animator.SetBool("bThumbsy", bThumbsy);

        bool bFormFist = Input.GetKey("3"); // 
        m_animator.SetBool("bFormFist", bFormFist);

        bool bAoK = Input.GetKey("4"); // 
        m_animator.SetBool("bAoK", bAoK);

        bool bUpYours = Input.GetKey("5"); // 
        m_animator.SetBool("bUpYours", bUpYours);

        bool bRockOn = Input.GetKey("6"); // 
        m_animator.SetBool("bRockOn", bRockOn);

        bool bShoot = Input.GetKey("7"); // 
        m_animator.SetBool("bShoot", bShoot);

        bool bThumbsUp = Input.GetKey("8"); // 
        m_animator.SetBool("bThumbsUp", bThumbsUp);

        bool bRangeTest = Input.GetKey("9"); // 
        m_animator.SetBool("bRangeTest", bRangeTest);

        bool bPoint = Input.GetKey("q"); // 
        m_animator.SetBool("bPoint", bPoint);

        bool bPeace = Input.GetKey("w"); // 
        m_animator.SetBool("bPeace", bPeace);

        bool bThree = Input.GetKey("e"); // 
        m_animator.SetBool("bThree", bThree);

        bool bFour = Input.GetKey("r"); // 
        m_animator.SetBool("bFour", bFour);

        bool bSpread = Input.GetKey("t"); // 
        m_animator.SetBool("bSpread", bSpread);

        bool bCallMe = Input.GetKey("y"); // 
        m_animator.SetBool("bCallMe", bCallMe);

        bool bHoldFlashlight = Input.GetKey("f"); // 
        m_animator.SetBool("bHoldFlashlight", bHoldFlashlight);


    }


}
