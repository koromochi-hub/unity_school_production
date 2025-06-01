using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMove : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private Animator animator;

    [Header("Movement Settings")]
    [SerializeField] private float runSpeed = 3f;
    [SerializeField] private float walkSpeed = 2.5f;
    [SerializeField] private float turnSpeed = 10f;

    private float speed;
    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation; // 回転を固定
        speed = runSpeed;
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.performed)
            moveInput = context.ReadValue<Vector2>();
        else if (context.canceled)
            moveInput = Vector2.zero;
    }

    public void OnSearch(InputAction.CallbackContext context)
    {
        if (context.performed)
            speed = walkSpeed;
        else if (context.canceled)
            speed = runSpeed;
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        RotateCharacter();
    }

    private void ApplyMovement()
    {
        // 水平方向の移動ベクトル
        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

        Vector3 velocity = move * speed;

        // 移動（Rigidbodyで直接位置を制御）
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    private void RotateCharacter()
    {
        Vector3 direction = new Vector3(moveInput.x, 0, moveInput.y);
        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime));
        }

        animator.SetBool("isRunning", direction.sqrMagnitude > 0.01f);
    }
}
