using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OpenDoor : MonoBehaviour
{
    public GameObject door;
    public GameObject Lock;
    private XRGrabInteractable interactable;

    //만약 Lock 이 소멸하면 door의  를 열어준다.
    // Start is called before the first frame update
    void Start()
    {
        interactable = GetComponent<XRGrabInteractable>();
        interactable.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Lock == null)
        {
            interactable.enabled = true;
        }
    }
}
