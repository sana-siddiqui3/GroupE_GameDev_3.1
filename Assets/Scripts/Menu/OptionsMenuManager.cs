using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenuManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider sensitivitySlider;
    public Slider volumeSlider;

    private int sensitivity;
    private int volume;

    // Reference to PlayerLook, but we will find it dynamically
    private PlayerLook playerLook;

    private void Start()
    {
        // Load saved settings or use default values
        volume = PlayerPrefs.GetInt("Volume", 100); // Default: 100
        sensitivity = PlayerPrefs.GetInt("Sensitivity", 50); // Default: 50

        // Initialize sliders
        sensitivitySlider.minValue = 0;
        sensitivitySlider.maxValue = 100;
        sensitivitySlider.value = sensitivity;

        volumeSlider.minValue = 0;
        volumeSlider.maxValue = 100;
        volumeSlider.value = volume;

        // Add listeners to sliders
        sensitivitySlider.onValueChanged.AddListener((value) => UpdateSensitivity(value));
        volumeSlider.onValueChanged.AddListener((value) => UpdateVolume(value));

        // Find the PlayerLook component in the scene
        playerLook = FindFirstObjectByType<PlayerLook>();

        // If PlayerLook is found, update its sensitivity with the saved value
        if (playerLook != null)
        {
            playerLook.UpdateSensitivity(sensitivity);
        }
        else
        {
            Debug.LogWarning("PlayerLook component not found in the scene!");
        }
    }

    private void UpdateSensitivity(float value)
    {
        sensitivity = Mathf.RoundToInt(value); // Convert to integer
        PlayerPrefs.SetInt("Sensitivity", sensitivity); // Save to PlayerPrefs
        PlayerPrefs.Save(); // Ensure data is saved

        // Update PlayerLook sensitivity if it's found
        if (playerLook != null)
        {
            playerLook.UpdateSensitivity(sensitivity);
        }

        Debug.Log($"Input Sensitivity updated to: {sensitivity}");
    }

    private void UpdateVolume(float value)
    {
        volume = Mathf.RoundToInt(value); // Convert to integer
        PlayerPrefs.SetInt("Volume", volume); // Save to PlayerPrefs
        PlayerPrefs.Save(); // Ensure data is saved

        // Scale volume to a 0.0 - 1.0 range and apply
        AudioListener.volume = volume / 100f;
        Debug.Log($"Volume updated to: {volume}");
    }

    public void ReturnToMainMenu()
    {
        // Load the main menu scene
        SceneManager.LoadScene("MainMenu");
        Debug.Log("Returning to Main Menu.");
    }
}
