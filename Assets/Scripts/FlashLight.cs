using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    bool playerGetLight = false;
    Light myLight; //light 컴포넌트를 담는 변수

    void Start()
    {
        playerGetLight = false;
        myLight = GetComponent<Light>();
    }

    void Update()
    {
        //작성중...
    }
}
