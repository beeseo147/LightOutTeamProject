using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private InputActionReference move;

    private void Update()
    {
        float moveX = move.action.ReadValue<Vector2>().x;
        animator.SetFloat("Horizontal", moveX);
        float moveY = move.action.ReadValue<Vector2>().y;
        animator.SetFloat("Vertical", moveY);
    }
}
