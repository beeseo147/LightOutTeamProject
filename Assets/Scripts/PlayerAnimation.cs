using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private InputActionReference move;

    private void OnEnable()
    {
        move.action.Enable();
        move.action.actionMap?.Enable(); // Áß¿ä!
    }

    private void OnDisable()
    {
        move.action.Disable();
    }

    private void Update()
    {
        float moveX = move.action.ReadValue<Vector2>().x;
        animator.SetFloat("Horizontal", moveX);
        float moveY = move.action.ReadValue<Vector2>().y;
        animator.SetFloat("Vertical", moveY);
    }
}
