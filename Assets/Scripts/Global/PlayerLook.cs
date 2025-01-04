using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    public Camera cam;
    private float xRotation = 0f;

    private PlayerInput controls;
    private InputAction mouseLookAction;

    private float xSensitivity = 100f; // Default sensitivity value
    private float ySensitivity = 100f; // Default sensitivity value

    void Awake()
    {
        // Get the Input Actions
        controls = new PlayerInput();
        mouseLookAction = controls.Player.MouseLook; // Assuming the action is named MouseLook under Player action map

        // Retrieve sensitivity from PlayerPrefs or OptionsMenuManager
        xSensitivity = PlayerPrefs.GetInt("Sensitivity", 50); // Default value 50
        ySensitivity = PlayerPrefs.GetInt("Sensitivity", 50); // Use the same for ySensitivity for uniformity
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
        // Get mouse input and process look
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

    // Add a method to update sensitivity from the OptionsMenuManager
    public void UpdateSensitivity(float newSensitivity)
    {
        xSensitivity = newSensitivity;
        ySensitivity = newSensitivity; // If you want both axes to use the same sensitivity
    }
}
