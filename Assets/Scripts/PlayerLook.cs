using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera cam;
    private float xRotation = 0f;

    public float xSensitivity = 100f;
    public float ySensitivity = 100f;
    private bool isMouseClicked = false; // Flag to check if the mouse button is clicked

    void Update()
    {
        // Check for mouse click
        if (Input.GetMouseButtonDown(0)) // Left mouse button clicked
        {
            isMouseClicked = true;
        }

        if (Input.GetMouseButtonUp(0)) // Left mouse button released
        {
            isMouseClicked = false;
        }
    }

    public void ProcessLook(Vector2 input)
    {
        if (!isMouseClicked) return; // Only process look if mouse button is clicked

        float mouseX = input.x * xSensitivity * Time.deltaTime;
        float mouseY = input.y * ySensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}
