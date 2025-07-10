using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadLockOpen : MonoBehaviour
{
    public Transform padLockRing;
    bool bisOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        padLockRing = transform.Find("PadlockRing");
    }
    
    public void OpenLock()
    {
        if(bisOpen == false)
        {
            padLockRing.GetComponent<Animator>().SetBool("isOpen", true);
            bisOpen = true;
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
