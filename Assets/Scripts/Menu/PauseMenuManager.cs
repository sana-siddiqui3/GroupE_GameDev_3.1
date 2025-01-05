using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu; // Reference to the pause menu UI
    private bool isPaused = false; // Tracks pause state

    [Header("UI Elements")]
    public Slider sensitivitySlider;
    public Slider volumeSlider;

    private float sensitivity;
    private int volume;

    private void Start()
    {
        pauseMenu.SetActive(false); // Ensure the inventory menu is initially hidden

        // Load saved settings or use default values
        volume = PlayerPrefs.GetInt("Volume", 100); // Default: 100
        sensitivity = PlayerPrefs.GetFloat("Sensitivity", 50f); // Default: 50

        // Initialize sliders
        sensitivitySlider.minValue = 10f; // Updated: Minimum sensitivity is now 1
        sensitivitySlider.maxValue = 100f; // Set a maximum sensitivity value
        sensitivitySlider.value = sensitivity;

        volumeSlider.minValue = 0;
        volumeSlider.maxValue = 100;
        volumeSlider.value = volume;

        // Add listeners to sliders
        sensitivitySlider.onValueChanged.AddListener(UpdateSensitivity);
        volumeSlider.onValueChanged.AddListener(UpdateVolume);
    }

    private void Update()
    {
        if (UnityEngine.InputSystem.Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        Debug.Log("Pause action detected!"); // Log for testing
        if (isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Resume()
    {
        Debug.Log("Resuming game..."); // Log for testing
        pauseMenu.SetActive(false); // Hide the pause menu
        Time.timeScale = 1f;          // Resume game time
        isPaused = false;             // Update pause state
    }

    public void Pause()
    {
        Debug.Log("Pausing game..."); // Log for testing
        pauseMenu.SetActive(true); // Show the pause menu
        Time.timeScale = 0f;         // Freeze game time
        isPaused = true;             // Update pause state
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

    public void QuitGame()
    {
        Debug.Log("Returning to Main Menu...");
        Resume(); // Ensure game is unpaused before returning to main menu
        SceneManager.LoadScene("MainMenu");
    }
}
