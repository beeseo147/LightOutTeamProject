using Photon.Pun;
using UnityEngine;

public class HandTargetFollower : MonoBehaviourPun
{
    public Transform source;
    public Vector3 eulerOffset = new(90, 0, 0);

    void LateUpdate()
    {
        if (photonView.IsMine && source != null)
            transform.SetPositionAndRotation(source.position, source.rotation * Quaternion.Euler(eulerOffset));
    }
}