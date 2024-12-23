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

    public GameObject fightStarted;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

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

            // Apply gravity to the player
            playerVelocity.y += gravity * Time.deltaTime;
            if (isGrounded && playerVelocity.y < 0)
            {
                playerVelocity.y = -2f;
            }
            controller.Move(playerVelocity * Time.deltaTime);
        }
        // During the fight, no movement processing occurs
    }

    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }
}
