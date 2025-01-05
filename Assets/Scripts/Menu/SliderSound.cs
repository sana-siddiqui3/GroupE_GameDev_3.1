using UnityEngine;
using UnityEngine.UI;

public class SliderSound : MonoBehaviour
{
    public AudioSource audioSource;  // Reference to the Audio Source
    public AudioClip sliderValueChangeSound;  // Sound for changing slider value
    public AudioClip sliderHoverSound;       // Sound for hovering over the slider

    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
        if (slider != null)
        {
            slider.onValueChanged.AddListener(PlayValueChangeSound);
        }
    }

    // Play sound when slider value changes
    private void PlayValueChangeSound(float value)
    {
        if (audioSource != null && sliderValueChangeSound != null)
        {
            audioSource.PlayOneShot(sliderValueChangeSound);
        }
    }

    // Play sound when hovering over the slider
    public void PlayHoverSound()
    {
        if (audioSource != null && sliderHoverSound != null)
        {
            audioSource.PlayOneShot(sliderHoverSound);
        }
    }
}
