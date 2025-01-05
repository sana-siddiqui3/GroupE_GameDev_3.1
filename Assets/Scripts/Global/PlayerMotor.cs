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

    public GameObject[] fightUIs; // Array of fight UI references

    // Reference to Input Actions
    private PlayerInput controls;
    private InputAction moveAction;
    private InputAction jumpAction;

    // Reference to the Animator for walking animation
    private Animator animator;

    // **Audio Fields**
    public AudioSource audioSource;       // Reference to the Audio Source
    public AudioClip footstepSound;       // Sound for footsteps
    public AudioClip jumpSound;           // Sound for jumping
    public AudioClip landingSound; // Sound for landing on the ground
    
    private bool wasGrounded;      // Tracks if the player was grounded in the previous frame

    private float footstepInterval = 0.4f; // Time between footstep sounds
    private float footstepTimer = 0f;     // Timer for footstep sounds

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>(); // Get the Animator component
        
        // Get the input actions from the PlayerControls asset
        controls = new PlayerInput();
        moveAction = controls.Player.Move;  // Move action
        jumpAction = controls.Player.Jump;  // Jump action

        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource is not assigned to PlayerMotor!");
        }
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

        // Detect landing
        if (!wasGrounded && isGrounded)
        {
            PlayLandingSound();
        }

        // Update wasGrounded for the next frame
        wasGrounded = isGrounded;

        // Check if any fight UI is active
        bool isFightActive = IsAnyFightUIActive();

        // If no fight is active, allow movement
        if (!isFightActive)
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

            // Play footstep sounds
            HandleFootstepSounds(input);
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

            // Play jump sound
            if (audioSource != null && jumpSound != null)
            {
                audioSource.PlayOneShot(jumpSound);
            }
        }
    }

    // Play footstep sounds while moving
    private void HandleFootstepSounds(Vector2 input)
    {
        if (isGrounded && input.magnitude > 0f) // Check if moving and grounded
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f)
            {
                if (audioSource != null && footstepSound != null)
                {
                    audioSource.PlayOneShot(footstepSound);
                }
                footstepTimer = footstepInterval; // Reset timer
            }
        }
        else
        {
            footstepTimer = footstepInterval; // Reset timer when not moving
        }
    }

    // Check if any fight UI is active
    private bool IsAnyFightUIActive()
    {
        foreach (GameObject fightUI in fightUIs)
        {
            if (fightUI.activeSelf)
            {
                return true;
            }
        }
        return false;
    }

    private void PlayLandingSound()
    {
        if (audioSource != null && landingSound != null)
        {
            audioSource.PlayOneShot(landingSound);
        }
    }

}
