using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    public Camera cam;
    private float xRotation = 0f;

    public float xSensitivity = 100f;
    public float ySensitivity = 100f;

    private PlayerInput controls;
    private InputAction mouseLookAction;

    // Reference to PlayerMotor to access the fight UI status
    public PlayerMotor playerMotor;

    private bool isMouseLookEnabled = true; // Variable to track if mouse look is enabled

    void Awake()
    {
        // Get the Input Actions
        controls = new PlayerInput();
        mouseLookAction = controls.Player.MouseLook; // Assuming the action is named MouseLook under Player action map
    }

    void OnEnable()
    {
        controls.Enable(); // Enable the action map
    }

    void OnDisable()
    {
        controls.Disable(); // Disable the action map
    }

    void Update()
    {
        // Check if any fight UI is active and disable/enable mouse look accordingly
        if (IsAnyFightUIActive())
        {
            isMouseLookEnabled = false; // Disable mouse look if fight UI is active
        }
        else
        {
            isMouseLookEnabled = true; // Enable mouse look if no fight UI is active
        }

        // Only allow mouse look if it's enabled
        if (isMouseLookEnabled)
        {
            Vector2 input = mouseLookAction.ReadValue<Vector2>(); 
            ProcessLook(input);
        }
    }

    private void ProcessLook(Vector2 input)
    {
        float mouseX = input.x * xSensitivity * Time.deltaTime;
        float mouseY = input.y * ySensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    // Checks if any fight UI is active
    private bool IsAnyFightUIActive()
    {
        foreach (GameObject fightUI in playerMotor.fightUIs)
        {
            if (fightUI.activeSelf)
            {
                return true;
            }
        }
        return false;
    }
}
