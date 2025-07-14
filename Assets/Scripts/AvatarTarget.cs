using Photon.Pun;
using UnityEngine;

public class AvatarTarget : MonoBehaviourPun
{
    public Transform source;
    public Vector3 positionOffset = new Vector3(0, -1.4f, 0); 
    public Vector3 eulerOffset;

    void LateUpdate()
    {
        if (!photonView.IsMine || source == null) return;

        // 위치를 HMD 기준에서 몸통 중심으로 내림
        transform.position = source.position + positionOffset;

        // 회전은 yaw만 따로 추출
        Vector3 forward = Vector3.ProjectOnPlane(source.forward, Vector3.up);
        if (forward.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
    }
}