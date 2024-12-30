using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // This method will be called when the Play button is pressed
    public void PlayGame()
    {
        // Load the game scene
        SceneManager.LoadScene("Room1");
    }

    // This method will be called when the Options button is pressed
    public void OpenOptions()
    {
        Debug.Log("Options menu opened."); // Placeholder
    }

    // This method will be called when the Quit button is pressed
    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit(); // Quits the application (won't work in the editor)
    }
}
