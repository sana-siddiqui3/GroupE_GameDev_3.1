using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private Vector3 moveDirection;

    public float rotationSpeed = 180f;
    public float speed = 15f;
    private Vector3 rotation;
    private bool isGrounded;

    private float gravity = -9.8f;
    private float jumpHeight = 2f;

    public GameObject fightStarted; // Reference to fightStarted object

    // Reference to Input Actions
    private PlayerInput controls;
    private InputAction moveAction;
    private InputAction jumpAction;

    // Reference to the Animator for walking animation
    private Animator animator;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>(); // Get the Animator component
        
        // Get the input actions from the PlayerControls asset
        controls = new PlayerInput();
        moveAction = controls.Player.Move;  // Move action
        jumpAction = controls.Player.Jump;  // Jump action
    }

    void OnEnable()
    {
        controls.Enable(); // Enable the input actions
    }

    void OnDisable()
    {
        controls.Disable(); // Disable the input actions
    }

    void Update()
    {
        isGrounded = controller.isGrounded;  // Check if the player is on the ground

        // If the player is not in a fight, allow movement
        if (!fightStarted.activeSelf)
        {
            Vector2 input = moveAction.ReadValue<Vector2>(); // Get movement input

            // Process player movement
            ProcessMove(input);

            // Apply gravity
            playerVelocity.y += gravity * Time.deltaTime;
            if (isGrounded && playerVelocity.y < 0)
            {
                playerVelocity.y = -2f;
            }
            controller.Move(playerVelocity * Time.deltaTime);
        }
        else
        {
            // Stop walking animation during fight
            animator.SetBool("isWalking", false);  // Ensure walking animation is not active during fight
        }

        // Check if the player presses the jump button
        if (jumpAction.triggered && isGrounded)
        {
            Jump();
        }
    }

    // Handles movement logic based on input
    public void ProcessMove(Vector2 input)
    {
        // Convert input to world space movement
        moveDirection.x = input.x;
        moveDirection.z = input.y;

        // If the player is moving, set isWalking to true
        bool isMoving = moveDirection.x != 0f || moveDirection.z != 0f;
        animator.SetBool("isWalking", isMoving);  // Set the walking animation state

        // Move the player
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
    }

    // Handles jumping
    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }
}
