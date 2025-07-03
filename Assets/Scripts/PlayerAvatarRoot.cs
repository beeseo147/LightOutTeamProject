using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatarRoot : MonoBehaviourPun
{
    [SerializeField] Transform hmd;          // Main Camera Transform
    [SerializeField] Transform avatarRoot;   // AvatarVisuals 루트
    [SerializeField] Vector3 hipOffset = new(0, -0.15f, 0); // 머리→엉덩이 오프셋

    void LateUpdate()
    {
        if (!photonView.IsMine) return;      // 본인만 계산, 리모트는 네트워크 값만 수신

        // 1. 위치 = HMD 세계 좌표 + 오프셋
        avatarRoot.position = hmd.position + hipOffset;

        // 2. 회전 = HMD의 수평 Yaw만 따라감
        Vector3 yaw = new(0f, hmd.eulerAngles.y, 0f);
        avatarRoot.rotation = Quaternion.Euler(yaw);
    }
}
