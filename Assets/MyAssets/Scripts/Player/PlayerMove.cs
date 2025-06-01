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
        rb.constraints = RigidbodyConstraints.FreezeRotation; // ��]���Œ�
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
        // ���������̈ړ��x�N�g��
        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

        Vector3 velocity = move * speed;

        // �ړ��iRigidbody�Œ��ڈʒu�𐧌�j
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
