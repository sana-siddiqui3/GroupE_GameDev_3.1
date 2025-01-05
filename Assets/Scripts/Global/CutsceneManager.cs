using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables; // For controlling Timeline playback

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private PlayableDirector playableDirector; // Assign in Inspector
    [SerializeField] private string firstRoomSceneName = "Room1"; // Name of the first room scene

    private void Start()
    {
        // Check if the player has already seen the intro cutscene
        if (PlayerPrefs.GetInt("HasSeenIntro", 0) == 1)
        {
            // Skip the cutscene and go directly to the first room
            LoadFirstRoom();
        }
        else
        {
            // Play the cutscene
            PlayCutscene();
        }
    }

    private void PlayCutscene()
    {
        // Start the Timeline from the beginning
        if (playableDirector != null)
        {
            playableDirector.Play();
        }

        // Register a callback to trigger when the Timeline ends
        playableDirector.stopped += OnCutsceneEnd;
    }

    private void OnCutsceneEnd(PlayableDirector director)
    {
        // Mark the cutscene as viewed
        PlayerPrefs.SetInt("HasSeenIntro", 1);
        PlayerPrefs.Save();

        // Load the first room
        LoadFirstRoom();
    }

    private void LoadFirstRoom()
    {
        // Load the first room scene
        SceneManager.LoadScene(firstRoomSceneName);
    }

    // Optional: Allow skipping the cutscene
    public void SkipCutscene()
    {
        if (playableDirector != null)
        {
            playableDirector.Stop(); // Stop the Timeline
        }

        OnCutsceneEnd(playableDirector); // Manually trigger the end callback
    }
}
