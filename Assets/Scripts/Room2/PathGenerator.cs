using UnityEngine;
using UnityEngine.SceneManagement;  // Required for scene management if you want to reload
using TMPro;

public class PathGenerator : MonoBehaviour
{
    public GameObject[] rows; // List of rows (each row has two child tiles)
    public GameObject player;  // Reference to the player object
    public Transform startingPosition; // Starting position (beginning of the bridge)
    private int triesRemaining = 4; // Track remaining tries
    private float fallPenalty; // Amount to reduce health by after each fall
    private bool isFirstFall = true; // Flag to check if it's the first fall

    private CharacterController characterController; // Reference to the CharacterController

    void Start()
    {
        // Get the CharacterController (if attached)
        characterController = player.GetComponent<CharacterController>();

        // Call to randomize the bridge
        ResetBridge();
    }

    void Update()
    {
        // Any other update logic if needed
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
        if (isFirstFall)
        {
            // Calculate the fall penalty (25% of the player's initial health) on the first fall
            fallPenalty = Mathf.RoundToInt(PlayerData.instance.playerHealth * 0.33f); // Round the penalty to nearest integer
            isFirstFall = false; // Set flag to false to ensure we don't recalculate
        }

        if (triesRemaining > 0)
        {
            // Reduce health by the stored fall penalty value (rounded)
            PlayerData.instance.playerHealth = Mathf.Max(0, Mathf.RoundToInt(PlayerData.instance.playerHealth - fallPenalty)); // Round the health

            // Decrease tries left
            triesRemaining--;
            Debug.Log($"Player fell! Health remaining: {PlayerData.instance.playerHealth}. Tries remaining: {triesRemaining}");

            // Update the health display
            PlayerData.instance.UpdateHealthDisplay();

            // Check if the player has health remaining
            if (PlayerData.instance.playerHealth <= 0)
            {
                GameOver();
            }
            else
            {
                ResetPlayerPosition();  // Reset the player's position if they still have tries left
            }
        }
        else
        {
            GameOver(); // Game over if no tries left
        }
    }

    // Handle game over scenario
    private void GameOver()
    {
        // Reload or end the game
        Debug.Log("Game Over!");
        SceneManager.LoadScene("MainMenu");
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
