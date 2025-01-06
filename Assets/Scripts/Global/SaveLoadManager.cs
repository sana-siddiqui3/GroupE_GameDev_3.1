using UnityEngine;
using System.IO;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager instance;

    // Optional: store the "current checkpoint" if you want to keep track of
    // which door was most recently used.
    [HideInInspector]
    public string currentCheckpoint;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Saves the game state, including PlayerData and checkpoint info.
    /// Call this from the DoorCheckpoint (or any script) when the player 
    /// interacts with a door.
    /// </summary>
    public void SaveGame()
    {
        // Grab your PlayerData single instance
        PlayerData pd = PlayerData.instance;
        if (pd == null)
        {
            Debug.LogError("PlayerData instance not found! Cannot save.");
            return;
        }

        // Prepare the data container
        SaveData data = new SaveData
        {
            // PlayerData fields
            playerHealth = pd.playerHealth,
            inventory = pd.inventory,
            keysCollected = pd.keysCollected,
            objective = pd.objective,
            difficulty = pd.difficulty,

            // Checkpoint data
            currentCheckpoint = currentCheckpoint
        };

        // Convert to JSON
        string json = JsonUtility.ToJson(data, prettyPrint: true);

        // Path to our save file
        string path = Path.Combine(Application.persistentDataPath, "savegame.json");
        File.WriteAllText(path, json);

        Debug.Log($"Game saved to: {path}");
    }

    /// <summary>
    /// Loads the game state from a JSON file and updates PlayerData + checkpoint info.
    /// </summary>
    public void LoadGame()
    {
        string path = Path.Combine(Application.persistentDataPath, "savegame.json");

        if (!File.Exists(path))
        {
            Debug.LogWarning("No save file found!");
            return;
        }

        // Read and deserialize
        string json = File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        // Update PlayerData
        PlayerData pd = PlayerData.instance;
        if (pd == null)
        {
            Debug.LogError("PlayerData instance not found! Cannot load.");
            return;
        }

        pd.playerHealth = data.playerHealth;
        pd.inventory = data.inventory;
        pd.keysCollected = data.keysCollected;
        pd.objective = data.objective;
        pd.difficulty = data.difficulty;

        // Update Checkpoint
        currentCheckpoint = data.currentCheckpoint;

        // Update UI or other game logic
        pd.UpdateKeyDisplay();
        pd.setObjective(pd.objective);

        Debug.Log($"Game loaded from: {path}");
    }
}
