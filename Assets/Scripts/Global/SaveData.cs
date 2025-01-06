using System.Collections.Generic;
using UnityEngine;

// Make sure this file is named "SaveData.cs"
[System.Serializable]
public class SaveData
{
    // ----------------------------
    // Fields From PlayerData
    // ----------------------------
    public float playerHealth;
    public List<InventoryItem> inventory;
    public int keysCollected;
    public string objective;
    public int difficulty;

    // ----------------------------
    // Checkpoint-Related Field
    // ----------------------------
    public string currentCheckpoint;
}
