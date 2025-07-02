//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.XR.Interaction.Toolkit;

//public class OculusInput : MonoBehaviour
//{
//    public HandAnimationController handAnim;

//    void Start()
//    {
//        var interactable = GetComponent<XRBaseInteractable>();
//        if (interactable != null)
//        {
//            interactable.firstHoverEntered.AddListener(OnFirstHoverEntered);
//            interactable.lastHoverExited.AddListener(OnLastHoverExited);
//            interactable.firstSelectEntered.AddListener(OnFirstSelectEntered);
//            interactable.lastSelectExited.AddListener(OnLastSelectExited);
//            interactable.activated.AddListener(OnActivated);
//            interactable.deactivated.AddListener(OnDeactivated);
//        }
//    }

//    public void OnFirstHoverEntered(HoverEnterEventArgs args)
//    {
//        Debug.Log("Hover ����");
//        handAnim.SetHandState(HandState.Point);
//    }

//    public void OnLastHoverExited(HoverExitEventArgs args)
//    {
//        Debug.Log("Hover ����");
//        handAnim.SetHandState(HandState.Default);
//    }

//    // Grip
//    public void OnFirstSelectEntered(SelectEnterEventArgs args)
//    {
//        Debug.Log("��� ����");
//        handAnim.SetHandState(HandState.GrabLight);
//    }

//    public void OnLastSelectExited(SelectExitEventArgs args)
//    {
//        Debug.Log("��� ����");
//        handAnim.SetHandState(HandState.Default);
//    }

//    // Trigger
//    public void OnActivated(ActivateEventArgs args)
//    {
//        Debug.Log("�� ��� �ߵ�!");
//        handAnim.SetHandState(HandState.GrabHard);
//    }

//    public void OnDeactivated(DeactivateEventArgs args)
//    {
//        Debug.Log("Activate ����");
//        handAnim.SetHandState(HandState.GrabLight);
//    }

//}

using UnityEngine;

public class OculusInput : MonoBehaviour
{
    private void OnEnable()
    {
        Debug.Log($"OnEnable");
    }

    public void OnFirstHoverEntered()
    {
        Debug.Log($"{gameObject.name} - OnFirstHoverEntered");
    }

    public void OnLastHoverExited()
    {
        Debug.Log($"{gameObject.name} - OnLastHoverExited");
    }

    public void OnHoverEntered()
    {
        Debug.Log($"{gameObject.name} - OnHoverEntered");
    }

    public void OnHoverExited()
    {
        Debug.Log($"{gameObject.name} - OnHoverExited");
    }

    public void OnFirstSelectEntered()
    {
        Debug.Log($"{gameObject.name} - OnFirstSelectEntered");
    }

    public void OnLastSelectExited()
    {
        Debug.Log($"{gameObject.name} - OnLastSelectExited");
    }

    public void OnSelectEntered()
    {
        Debug.Log($"{gameObject.name} - OnSelectEntered");
    }

    public void OnSelectExited()
    {
        Debug.Log($"{gameObject.name} - OnSelectExited");
    }

    public void OnActivated()
    {
        Debug.Log($"{gameObject.name} - OnActivated");
    }

    public void OnDeactivated()
    {
        Debug.Log($"{gameObject.name} - OnDeactivated");
    }
}
