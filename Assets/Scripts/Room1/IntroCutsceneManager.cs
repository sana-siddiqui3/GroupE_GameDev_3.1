using UnityEngine;
using UnityEngine.Playables;       // Required for Timeline
using UnityEngine.SceneManagement; // Required for loading scenes

public class IntroCutsceneManager : MonoBehaviour
{
    [SerializeField] private PlayableDirector playableDirector;

    private void Start()
    {
        // Check if the player has already seen the intro cutscene
        if (PlayerPrefs.GetInt("HasSeenIntro", 0) == 0)
        {
            // Player hasn't seen it yet—play the timeline
            playableDirector.Play();
        }
        else
        {
            // Player has seen the intro, skip to the main menu
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void OnEnable()
    {
        // Subscribe to the Timeline's "stopped" event
        playableDirector.stopped += OnCutsceneFinished;
    }

    private void OnDisable()
    {
        // Unsubscribe to avoid memory leaks
        playableDirector.stopped -= OnCutsceneFinished;
    }

    private void OnCutsceneFinished(PlayableDirector director)
    {
        // Mark intro as seen
        PlayerPrefs.SetInt("HasSeenIntro", 1);
        PlayerPrefs.Save();

        // Load the next scene
        SceneManager.LoadScene("MainMenu");
    }
}
