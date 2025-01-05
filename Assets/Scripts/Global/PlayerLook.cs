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

    public PlayerMotor playerMotor;

    private bool isMouseLookEnabled = true;

    void Awake()
    {
        controls = new PlayerInput();
        mouseLookAction = controls.Player.MouseLook;

        // Load sensitivity from PlayerPrefs
        float savedSensitivity = PlayerPrefs.GetFloat("Sensitivity", 100f);
        xSensitivity = savedSensitivity;
        ySensitivity = savedSensitivity;
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void Update()
    {
        if (IsAnyFightUIActive())
        {
            isMouseLookEnabled = false;
        }
        else
        {
            isMouseLookEnabled = true;
        }

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
