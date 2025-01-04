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
        // Get the mouse delta for movement
        Vector2 input = mouseLookAction.ReadValue<Vector2>(); 
        ProcessLook(input);
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
}
