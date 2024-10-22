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
        fightingPosition = player.transform.position;
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;
    }

    public void ProcessMove(Vector2 input)
    {
        if (!fightStarted.activeSelf)
        {
            Vector3 moveDirection = Vector3.zero;
            moveDirection.x = input.x;
            moveDirection.z = input.y;
            controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);

            this.rotation = new Vector3(0, Input.GetAxisRaw("Horizontal") * rotationSpeed * Time.deltaTime, 0);
            this.transform.Rotate(this.rotation);

            playerVelocity.y += gravity * Time.deltaTime;
            if (isGrounded && playerVelocity.y < 0)
            {
                playerVelocity.y = -2f;
            }
            controller.Move(playerVelocity * Time.deltaTime);
            //Debug.Log(playerVelocity.y);
        }
        else
        {
            player.transform.position = fightingPosition;
        }
    }

    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }
}
