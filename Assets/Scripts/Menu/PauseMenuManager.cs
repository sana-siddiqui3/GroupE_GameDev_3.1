using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    private bool isPaused = false;

    [Header("UI Elements")]
    public Slider sensitivitySlider;
    public Slider volumeSlider;

    private float sensitivity;
    private int volume;
    
    private PlayerLook playerLook;

    void Start()
    {
        pauseMenu.SetActive(false);

        // Load saved settings or use default values
        volume = PlayerPrefs.GetInt("Volume", 100);
        sensitivity = PlayerPrefs.GetFloat("Sensitivity", 100f);

        // Initialize sliders
        sensitivitySlider.minValue = 10f;
        sensitivitySlider.maxValue = 100f;
        sensitivitySlider.value = sensitivity;

        volumeSlider.minValue = 0;
        volumeSlider.maxValue = 100;
        volumeSlider.value = volume;

        // Add listeners to sliders
        sensitivitySlider.onValueChanged.AddListener(UpdateSensitivity);
        volumeSlider.onValueChanged.AddListener(UpdateVolume);

        // Get the PlayerLook component to update sensitivity
        playerLook = FindObjectOfType<PlayerLook>();  // Make sure to get the PlayerLook component from the scene
        if (playerLook != null)
        {
            playerLook.UpdateSensitivity(sensitivity); // Set initial sensitivity
        }
    }

    void Update()
    {
        if (UnityEngine.InputSystem.Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
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
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    private void UpdateSensitivity(float value)
    {
        sensitivity = value;
        PlayerPrefs.SetFloat("Sensitivity", sensitivity);
        PlayerPrefs.Save();

        // Update the PlayerLook script's sensitivity
        if (playerLook != null)
        {
            playerLook.UpdateSensitivity(sensitivity);
        }

        Debug.Log($"Input Sensitivity updated to: {sensitivity}");
    }

    private void UpdateVolume(float value)
    {
        volume = (int)value;
        PlayerPrefs.SetInt("Volume", volume);
        PlayerPrefs.Save();

        AudioListener.volume = volume / 100f;
        Debug.Log($"Volume updated to: {volume}");
    }

    public void QuitGame()
    {
        Resume();
        SceneManager.LoadScene("MainMenu");
    }
}
