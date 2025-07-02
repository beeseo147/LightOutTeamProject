using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class XROculusDebug : MonoBehaviour
{
    void Update()
    {
        var right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        if (right.TryGetFeatureValue(CommonUsages.trigger, out float trig))
            Debug.Log($"Trigger : {trig:F2}");
        if (right.TryGetFeatureValue(CommonUsages.grip, out float grip))
            Debug.Log($"Grip    : {grip:F2}");
    }
}
