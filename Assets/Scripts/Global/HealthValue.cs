using UnityEngine;
using UnityEngine.UI;
using TMPro;  // Include the TMP namespace

public class HealthSlider : MonoBehaviour
{
    public Slider healthSlider;  // Reference to the health slider
    public TMP_Text healthText;  // Reference to the TMP text that will display the health value

    void Start()
    {
        // Ensure the initial value is displayed
        UpdateHealthText();

        // Add a listener to update the text when the slider value changes
        healthSlider.onValueChanged.AddListener(delegate { UpdateHealthText(); });
    }

    void UpdateHealthText()
    {
        // Update the health text with the current value of the slider
        healthText.text = Mathf.RoundToInt(healthSlider.value).ToString();
    }
}
