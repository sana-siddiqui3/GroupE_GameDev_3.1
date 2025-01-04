using UnityEngine;
using UnityEngine.SceneManagement;  // Required for scene management if you want to reload

public class PathGenerator : MonoBehaviour
{
    public GameObject[] rows; // List of rows (each row has two child tiles)
    public GameObject player;  // Reference to the player object
    public int totalLives = 3; // Total lives
    private int currentLives; // Current lives

    public Transform startingPosition; // Starting position (beginning of the bridge)

    private CharacterController characterController; // Reference to the CharacterController

    void Start()
    {
        currentLives = totalLives; // Set lives at the start
        ResetBridge();  // Randomize tiles on startup

        // Get the CharacterController (if attached)
        characterController = player.GetComponent<CharacterController>();
    }

    void Update()
    {
        // Check if the player is out of lives
        if (currentLives <= 0)
        {
            // Optional: Display Game Over and Reload Level
            Debug.Log("Game Over!");
            Debug.Log("Returning to Main Menu.");
        }
    }

    // Randomizes the bridge tiles on start or reset
    void ResetBridge()
    {
        foreach (GameObject row in rows)
        {
            // Get child objects
            Transform[] children = row.GetComponentsInChildren<Transform>();

            // Ensure there are exactly 2 children (excluding the parent row object)
            if (children.Length != 3) // First is the parent itself
            {
                Debug.LogWarning($"Row '{row.name}' does not have exactly 2 children.");
                continue;
            }

            // Randomly pick one child to be the safe tile
            int safeIndex = Random.Range(1, 3); // 1 and 2 correspond to the children
            for (int i = 1; i < children.Length; i++) // Start at 1 to skip the parent
            {
                BoxCollider collider = children[i].GetComponent<BoxCollider>();
                if (collider != null)
                {
                    collider.isTrigger = (i != safeIndex); // Set unsafe tiles as triggers
                }
            }
        }
    }

    public void PlayerFell()
    {
        // Handle the player's fall
        currentLives--;  // Decrease life
        Debug.Log("Player fell! Lives remaining: " + currentLives);

        if (currentLives > 0)
        {
            ResetPlayerPosition();
        }
        else
        {
            // Reload the scene or display game over
            SceneManager.LoadScene("MainMenu");
            Debug.Log("Returning to Main Menu.");
        }
    }

    // Reset player position to the beginning of the bridge
    void ResetPlayerPosition()
    {
        // Disable movement for a short time to ensure the player position is reset properly
        if (characterController != null)
        {
            // Temporarily disable movement by stopping the CharacterController
            characterController.enabled = false;
        }

        // Reset the player's position to the starting position
        player.transform.position = startingPosition.position;

        // Enable movement again after a short delay
        if (characterController != null)
        {
            characterController.enabled = true;
        }
    }
}
