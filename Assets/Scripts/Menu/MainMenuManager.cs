using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider difficultySlider;

    private int difficulty;   

    private void Start()
    {
        // Load saved settings or use default values
        difficulty = PlayerPrefs.GetInt("Difficulty", 1); // Default: 1

        // Initialize sliders
        difficultySlider.minValue = 0;
        difficultySlider.maxValue = 2;
        difficultySlider.value = difficulty;

        // Add listeners to sliders
        difficultySlider.onValueChanged.AddListener((value) => UpdateDifficulty((int)value));
    }

    // Method to update the difficulty setting
    private void UpdateDifficulty(int value)
    {
        difficulty = value;
        PlayerPrefs.SetInt("Difficulty", difficulty); // Save to PlayerPrefs
        PlayerPrefs.Save(); // Ensure data is saved
        Debug.Log($"Difficulty updated to: {difficulty}");
    }

    // This method will be called when the Play button is pressed
    public void PlayGame()
    {
        // Load the game scene
        SceneManager.LoadScene("Room1");
    }

    // This method will be called when the Options button is pressed
    public void OpenOptions()
    {
        // Load the options menu scene
        SceneManager.LoadScene("OptionsMenu");
        Debug.Log("Options menu opened.");
    }
}
