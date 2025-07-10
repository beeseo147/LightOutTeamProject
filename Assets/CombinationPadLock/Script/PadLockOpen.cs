using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadLockOpen : MonoBehaviour
{
    public GameObject padLockRing;
    bool bisOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        padLockRing = GameObject.Find("PadlockRing");
    }
    
    public void OpenLock()
    {
        if(bisOpen == false)
        {
            padLockRing.transform.localPosition += new Vector3(0,0.1f,0);
            bisOpen = true;
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
