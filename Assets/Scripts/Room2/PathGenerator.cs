using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management
using TMPro;

public class PathGenerator : MonoBehaviour
{
    public GameObject[] rows; // List of rows (each row has two child tiles)
    public GameObject player; // Reference to the player object
    public Transform startingPosition; // Starting position (beginning of the bridge)
    public int difficulty = 1; // Difficulty level: 1 (easy), 2 (medium), 3 (hard)
    private int triesRemaining = 4; // Track remaining tries
    private float initialFallPenalty; // Amount to reduce health by after the first fall
    private float currentFallPenalty; // Fixed penalty to use after the first fall

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

    // Adjusts the fall penalty based on difficulty level
    void AdjustFallPenalty()
    {
        switch (difficulty)
        {
            case 1: // Easy
                initialFallPenalty = 0.10f; // 10% of health
                break;
            case 2: // Medium
                initialFallPenalty = 0.25f; // 25% of health
                break;
            case 3: // Hard
                initialFallPenalty = 0.33f; // 33% of health
                break;
            default:
                Debug.LogWarning("Invalid difficulty level. Defaulting to medium.");
                initialFallPenalty = 0.25f;
                break;
        }
    }

    // Randomizes the bridge tiles on start or reset
    void ResetBridge()
    {
        foreach (GameObject row in rows)
        {
            Transform[] children = row.GetComponentsInChildren<Transform>();
            if (children.Length != 3) // Ensure 2 child tiles
            {
                Debug.LogWarning($"Row '{row.name}' does not have exactly 2 children.");
                continue;
            }

            int safeIndex = Random.Range(1, 3); // Randomly select safe tile
            for (int i = 1; i < children.Length; i++)
            {
                BoxCollider collider = children[i].GetComponent<BoxCollider>();
                if (collider != null)
                {
                    collider.isTrigger = (i != safeIndex); // Unsafe tiles are triggers
                }
            }
        }

        // Apply the initial penalty adjustment at the start
        AdjustFallPenalty();
    }

    public void PlayerFell()
    {
        if (triesRemaining > 0)
        {
            // If the player is falling for the first time, calculate the penalty
            if (currentFallPenalty == 0)
            {
                // Apply the fall penalty based on health and difficulty
                currentFallPenalty = PlayerData.instance.playerHealth * initialFallPenalty;
            }

            // Apply the fixed penalty after the first fall
            PlayerData.instance.playerHealth = Mathf.Max(0, Mathf.RoundToInt(PlayerData.instance.playerHealth - currentFallPenalty));

            triesRemaining--;
            Debug.Log($"Player fell! Health: {PlayerData.instance.playerHealth}, Tries: {triesRemaining}");

            PlayerData.instance.UpdateHealthDisplay();

            if (PlayerData.instance.playerHealth <= 0)
            {
                GameOver();
            }
            else
            {
                ResetPlayerPosition();
            }
        }
        else
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");
        SceneManager.LoadScene("MainMenu");
    }

    void ResetPlayerPosition()
    {
        if (characterController != null) characterController.enabled = false;

        player.transform.position = startingPosition.position;

        if (characterController != null) characterController.enabled = true;
    }
}
