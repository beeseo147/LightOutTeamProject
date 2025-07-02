using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class guiMessage : MonoBehaviour {

    // spin around a gameobject



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame

    // screen message
    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 1920, 20), "FirstPersonHands - VR Compatible - Animations and Poses - Version 1.0 - RRFreelance - Unity Publisher 884");
        GUI.Label(new Rect(10, 30, 1920, 20), "Press 1-9 and 0 (Neutral) for Poses, Press Q,W,E,R,T for Finger Points/Counts");
        GUI.Label(new Rect(10, 50, 1920, 20), "Press Y for 'CallMe' Pose");
        GUI.Label(new Rect(10, 70, 1920, 20), "Press F for Holding a Flashlight Pose");
        GUI.Label(new Rect(10, 100, 1920, 20), "Arrow Key Up/Down to cycle the Skin Materials");
    }
}

