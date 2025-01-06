using UnityEngine;
using UnityEngine.SceneManagement; // If you’re using scene loading

public class DoorCheckpoint : MonoBehaviour
{
    [Header("Scene to Load")]
    [SerializeField] private string nextSceneName = "NextRoomScene";

    [Header("Interaction Key")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    private bool playerInRange = false;

    private void Update()
    {
        // Check for interaction only when the player is within range
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            // 1. Save the game
            SaveLoadManager.instance.SaveGame();

            // 2. Transition to the next room (e.g., load next scene)
            if (!string.IsNullOrEmpty(nextSceneName))
            {
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                Debug.LogWarning("No scene name assigned in DoorCheckpoint script!");
            }
        }
    }

    // This assumes you use triggers on your door area:
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player entered door trigger. Press E to open door and save game.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player exited door trigger.");
        }
    }
}

