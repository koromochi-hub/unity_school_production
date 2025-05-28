using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;
    [SerializeField] private Animator animator;

    [Header("Movement Settings")]
    [SerializeField] private float runSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float gravityScale = 9.81f;

    private float speed;
    private float verticalVelocity;
    private Vector2 moveInput;
    private Vector3 moveDirection;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
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

    private void Update()
    {
        ApplyMovement();
        RotateCharacter();
    }

    private void ApplyMovement()
    {
        moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);
        ApplyGravity();
        characterController.Move(moveDirection * Time.deltaTime * speed);
    }

    private void ApplyGravity()
    {
        if (!characterController.isGrounded)
            verticalVelocity -= gravityScale * Time.deltaTime;
        else
            verticalVelocity = -0.5f;

        moveDirection.y = verticalVelocity;
    }

    private void RotateCharacter()
    {
        if (moveInput.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(moveInput.x, 0, moveInput.y));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

        animator.SetBool("isRunning", moveInput.sqrMagnitude > 0.01f);
    }
}
