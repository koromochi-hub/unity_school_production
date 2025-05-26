using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerControls controls;
    private CharacterController characterController;
    [SerializeField] private Animator animator;

    [Header("Movement info")]
    [SerializeField] private float runSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float gravityScale = 9.81f;

    private float speed;
    private float verticalVelocity;

    private Vector2 moveInput;
    private Vector3 moveDirection;

    private bool isRunning;

    private void Awake()
    {
        AssignInputEvents();
    }

    private void Start()
    {
        characterController = GetComponent<CharacterController>();

        animator = GetComponentInChildren<Animator>();

        speed = runSpeed;
    }

    private void Update()
    {
        ApplyMovement();
        RotateTowardsMoveDirection();
    }

    private void ApplyMovement()
    {
        moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
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

    private void RotateTowardsMoveDirection()
    {
        isRunning = moveInput.sqrMagnitude > 0.01f;
        
        if (isRunning)
        {
            Vector3 lookDirection = new Vector3(moveInput.x, 0f, moveInput.y);
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

        animator.SetBool("isRunning", isRunning);
    }

    private void AssignInputEvents()
    {
        controls = new PlayerControls();

        controls.Character.Run.performed += context => moveInput = context.ReadValue<Vector2>();
        controls.Character.Run.canceled += context => moveInput = Vector2.zero;

        controls.Character.Search.performed += context =>
        {
            speed = walkSpeed;
            isRunning = false;
        };


        controls.Character.Search.canceled += context =>
        {
            speed = runSpeed;
            isRunning = true;
        };
    }


    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
