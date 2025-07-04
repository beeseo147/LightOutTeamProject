using UnityEngine;

public class AvatarRotationSync : MonoBehaviour
{
    public Transform cameraTransform;  // Main Camera
    void LateUpdate()
    {
        var yRot = cameraTransform.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0, yRot, 0);
    }
}