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

    private float sensitivity;
    private int volume;

    private void Start()

    {
        // Load saved settings or use default values
        volume = PlayerPrefs.GetInt("Volume", 100); // Default: 100
        sensitivity = PlayerPrefs.GetFloat("Sensitivity", 50f); // Default: 50

        // Initialize sliders
        sensitivitySlider.minValue = 10f; 
        sensitivitySlider.maxValue = 100f; // Set a maximum sensitivity value
        sensitivitySlider.value = sensitivity;

        volumeSlider.minValue = 0;
        volumeSlider.maxValue = 100;
        volumeSlider.value = volume;

        // Add listeners to sliders
        sensitivitySlider.onValueChanged.AddListener(UpdateSensitivity);
        volumeSlider.onValueChanged.AddListener(UpdateVolume);
    }

    private void UpdateSensitivity(float value)
    {
        sensitivity = value;
        PlayerPrefs.SetFloat("Sensitivity", sensitivity); // Save to PlayerPrefs
        PlayerPrefs.Save(); // Ensure data is saved
        Debug.Log($"Input Sensitivity updated to: {sensitivity}");
    }

    private void UpdateVolume(float value)
    {
        volume = (int)value;
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
