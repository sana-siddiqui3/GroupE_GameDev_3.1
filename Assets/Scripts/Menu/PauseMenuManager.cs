using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu; // Reference to the pause menu UI
    private bool isPaused = false; // Tracks pause state

    private void Start()
    {
        pauseMenu.SetActive(false); // Ensure the inventory menu is initially hidden
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

    public void QuitGame()
    {
        Debug.Log("Returning to Main Menu...");
        Resume(); // Ensure game is unpaused before returning to main menu
        SceneManager.LoadScene("MainMenu");
    }
}
