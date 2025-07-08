using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatarRoot : MonoBehaviourPun
{
    [SerializeField] Transform hmd;          // Main Camera Transform
    [SerializeField] Transform avatarRoot;   // AvatarVisuals ��Ʈ
    [SerializeField] Vector3 hipOffset = new(0, -0.15f, 0); // �Ӹ�������� ������

    void LateUpdate()
    {
        if (!photonView.IsMine) return;      // ���θ� ���, ����Ʈ�� ��Ʈ��ũ ���� ����

        // 1. ��ġ = HMD ���� ��ǥ + ������
        avatarRoot.position = hmd.position + hipOffset;

        // 2. ȸ�� = HMD�� ���� Yaw�� ����
        Vector3 yaw = new(0f, hmd.eulerAngles.y, 0f);
        avatarRoot.rotation = Quaternion.Euler(yaw);
    }
}
