using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(ActionBasedContinuousTurnProvider))]
public class TurnDebug : MonoBehaviour
{
    ActionBasedContinuousTurnProvider tp;     // Snap이면 타입 변경
    void Awake() => tp = GetComponent<ActionBasedContinuousTurnProvider>();

    void Update()
    {
        var act = tp.rightHandTurnAction.action;
        if (act == null || !act.enabled) return;

        Vector2 v = act.ReadValue<Vector2>();
        if (Mathf.Abs(v.x) > .01f)          // 값이 변하면 한 줄 출력
            Debug.Log($"TURN Action: {v}");
    }
}