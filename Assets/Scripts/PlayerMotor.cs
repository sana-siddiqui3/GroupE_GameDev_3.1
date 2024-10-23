using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    public float rotationSpeed = 180f;
    public float speed = 15f;
    private Vector3 rotation;
    private bool isGrounded;
    private float gravity = -9.8f;
    private float jumpHeight = 3f;

    private Vector3 fightingPosition;
    public GameObject player;
    public GameObject fightStarted;

    // Start is called before the first frame update
    void Awake()
    {
        fightingPosition = player.transform.position; // Save the position of the player before the fight starts
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded; // Check if the player is on the ground
    }

    public void ProcessMove(Vector2 input)
    {
        if (!fightStarted.activeSelf)
        {
            // Move the player
            Vector3 moveDirection = Vector3.zero;
            moveDirection.x = input.x;
            moveDirection.z = input.y;
            controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);

            // Rotate the player
            this.rotation = new Vector3(0, Input.GetAxisRaw("Horizontal") * rotationSpeed * Time.deltaTime, 0);
            this.transform.Rotate(this.rotation);

            // Apply gravity to the player
            playerVelocity.y += gravity * Time.deltaTime;
            if (isGrounded && playerVelocity.y < 0)
            {
                playerVelocity.y = -2f;
            }
            controller.Move(playerVelocity * Time.deltaTime);
        }
        else // If the fight has started, move the player to the fighting position
        {
            player.transform.position = fightingPosition;
        }
    }

    // Make the player jump
    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }
}
