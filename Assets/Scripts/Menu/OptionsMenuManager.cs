using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenuManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider sensitivitySlider;
    public Slider volumeSlider;

    private void Start()
    {
        // Load saved settings or use default values
        int volume = PlayerPrefs.GetInt("Volume", 100);
        float sensitivity = PlayerPrefs.GetFloat("Sensitivity", 100f);

        // Initialize sliders
        sensitivitySlider.minValue = 0f;
        sensitivitySlider.maxValue = 200f; // Adjust range as needed
        sensitivitySlider.value = sensitivity;

        volumeSlider.minValue = 0;
        volumeSlider.maxValue = 100;
        volumeSlider.value = volume;

        // Add listeners to sliders
        sensitivitySlider.onValueChanged.AddListener((value) => UpdateSensitivity(value));
        volumeSlider.onValueChanged.AddListener((value) => UpdateVolume((int)value));
    }

    private void UpdateSensitivity(float value)
    {
        PlayerPrefs.SetFloat("Sensitivity", value);
        PlayerPrefs.Save();
        Debug.Log($"Sensitivity updated to: {value}");
    }

    private void UpdateVolume(int value)
    {
        PlayerPrefs.SetInt("Volume", value);
        PlayerPrefs.Save();
        AudioListener.volume = value / 100f;
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
