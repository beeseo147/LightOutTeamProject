using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetworkSync : MonoBehaviourPun, IPunObservable
{
    [SerializeField] Transform leftHandController;
    [SerializeField] Transform rightHandController;
    [SerializeField] Transform head;
    [SerializeField] Transform avatarRoot; // for Body Rotation
    [SerializeField] Transform xrOrigin;

    Vector3 xrOriginLocalPos;
    Quaternion xrOriginLocalRot;

    Vector3 leftHandPos;
    Quaternion leftHandRot;

    Vector3 rightHandPos;
    Quaternion rightHandRot;

    Vector3 playerPos;
    Quaternion playerRot;

    Vector3 headPos;
    Quaternion headRot;

    void Update()
    {
        if (photonView.IsMine)
        {
            leftHandPos = leftHandController.localPosition;
            leftHandRot = leftHandController.localRotation;

            rightHandPos = rightHandController.localPosition;
            rightHandRot = rightHandController.localRotation;

            headPos = head.localPosition;
            headRot = head.localRotation;

            xrOriginLocalPos = xrOrigin.localPosition;
            xrOriginLocalRot = xrOrigin.localRotation;

            playerPos = transform.position;
            playerRot = transform.rotation;

            // local body rotation
            float yRot = head.eulerAngles.y;
            avatarRoot.rotation = Quaternion.Euler(0, yRot, 0);
        }
        else
        {
            leftHandController.localPosition = leftHandPos;
            leftHandController.localRotation = leftHandRot;

            rightHandController.localPosition = rightHandPos;
            rightHandController.localRotation = rightHandRot;

            head.localPosition = headPos;
            head.localRotation = headRot;

            transform.position = playerPos;
            transform.rotation = playerRot;

            //xrOrigin.localPosition = xrOriginLocalPos;
            //xrOrigin.localRotation = xrOriginLocalRot;
            xrOrigin.position = playerPos + playerRot * xrOriginLocalPos;
            xrOrigin.rotation = playerRot * xrOriginLocalRot;

            // remote body rotation
            //float yRot = headRot.eulerAngles.y;
            //avatarRoot.rotation = Quaternion.Euler(0, yRot, 0);
        }

        //transform.position = xrOrigin.position;
        //transform.rotation = xrOrigin.rotation;
        // **몸통 위치·회전 항상 마지막에 맞추기**
        avatarRoot.position = xrOrigin.position;
        avatarRoot.rotation = Quaternion.Euler(0, head.rotation.eulerAngles.y, 0);

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(leftHandPos);
            stream.SendNext(leftHandRot);

            stream.SendNext(rightHandPos);
            stream.SendNext(rightHandRot);

            stream.SendNext(headPos);
            stream.SendNext(headRot);

            stream.SendNext(playerPos);
            stream.SendNext(playerRot);

            stream.SendNext(xrOriginLocalPos);
            stream.SendNext(xrOriginLocalRot);
        }
        else
        {
            leftHandPos = (Vector3)stream.ReceiveNext();
            leftHandRot = (Quaternion)stream.ReceiveNext();

            rightHandPos = (Vector3)stream.ReceiveNext();
            rightHandRot = (Quaternion)stream.ReceiveNext();

            headPos = (Vector3)stream.ReceiveNext();
            headRot = (Quaternion)stream.ReceiveNext();

            playerPos = (Vector3)stream.ReceiveNext();
            playerRot = (Quaternion)stream.ReceiveNext();

            xrOriginLocalPos = (Vector3)stream.ReceiveNext();
            xrOriginLocalRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
