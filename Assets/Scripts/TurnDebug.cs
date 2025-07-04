using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(ActionBasedContinuousTurnProvider))]
public class TurnDebug : MonoBehaviour
{
    ActionBasedContinuousTurnProvider tp;     // Snap�̸� Ÿ�� ����
    void Awake() => tp = GetComponent<ActionBasedContinuousTurnProvider>();

    void Update()
    {
        var act = tp.rightHandTurnAction.action;
        if (act == null || !act.enabled) return;

        Vector2 v = act.ReadValue<Vector2>();
        if (Mathf.Abs(v.x) > .01f)          // ���� ���ϸ� �� �� ���
            Debug.Log($"TURN Action: {v}");
    }
}