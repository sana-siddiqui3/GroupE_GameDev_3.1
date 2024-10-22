using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    private Vector3 checkpointPosition;
    private bool checkpointActivated = false;

    // Detect when the player enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActivateCheckpoint(other.gameObject);
        }
    }

    // Function to activate the checkpoint
    private void ActivateCheckpoint(GameObject player)
    {
        checkpointPosition = transform.position;  // Set checkpoint position to the trigger's position
        checkpointActivated = true;

        // Save the checkpoint position (you can save more data here)
        SaveCheckpointData();

        Debug.Log("Checkpoint activated at position: " + checkpointPosition);
    }

    // Method to save checkpoint data (you can add more info to save)
    private void SaveCheckpointData()
    {
        // Example: Saving the checkpoint position using PlayerPrefs
        PlayerPrefs.SetFloat("CheckpointX", checkpointPosition.x);
        PlayerPrefs.SetFloat("CheckpointY", checkpointPosition.y);
        PlayerPrefs.SetFloat("CheckpointZ", checkpointPosition.z);
        PlayerPrefs.SetInt("CheckpointActivated", checkpointActivated ? 1 : 0);

        PlayerPrefs.Save();
        Debug.Log("Checkpoint data saved.");
    }

    // Method to load the checkpoint data and respawn player at the last checkpoint
    public void LoadCheckpoint(GameObject player)
    {
        if (PlayerPrefs.GetInt("CheckpointActivated", 0) == 1)  // Check if a checkpoint was activated
        {
            float x = PlayerPrefs.GetFloat("CheckpointX");
            float y = PlayerPrefs.GetFloat("CheckpointY");
            float z = PlayerPrefs.GetFloat("CheckpointZ");

            Vector3 respawnPosition = new Vector3(x, y, z);

            // Move the player to the last checkpoint
            player.transform.position = respawnPosition;
            Debug.Log("Player respawned at checkpoint position: " + respawnPosition);
        }
        else
        {
            Debug.Log("No checkpoint activated yet.");
        }
    }

    // Optional method to reset checkpoint data (for restarting the game)
    public void ResetCheckpointData()
    {
        PlayerPrefs.DeleteKey("CheckpointX");
        PlayerPrefs.DeleteKey("CheckpointY");
        PlayerPrefs.DeleteKey("CheckpointZ");
        PlayerPrefs.DeleteKey("CheckpointActivated");
        PlayerPrefs.Save();

        Debug.Log("Checkpoint data reset.");
    }
}

