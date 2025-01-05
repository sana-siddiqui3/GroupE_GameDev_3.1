using UnityEngine;
using System.Collections.Generic;

public class GameDataManager : MonoBehaviour
{
    // ------------------------------------------------
    // 1) Save the game
    // ------------------------------------------------
    public void SaveGame()
    {
        // Make sure PlayerData is available
        if (PlayerData.instance == null)
        {
            Debug.LogError("PlayerData instance is null! Cannot save.");
            return;
        }

        // Create the SaveData container
        SaveData data = new SaveData();

        // Read data from PlayerData (no changes to PlayerData needed!)
        data.playerHealth = PlayerData.instance.playerHealth;
        data.keysCollected = PlayerData.instance.keysCollected;
        data.objective = PlayerData.instance.objective;
        data.difficulty = PlayerData.instance.difficulty;

        // Copy item names from the existing inventory
        data.inventoryItemNames = new List<string>();
        foreach (var item in PlayerData.instance.inventory)
        {
            data.inventoryItemNames.Add(item.itemName);
        }

        // Use the SaveSystem to write JSON
        SaveSystem.Save(data);
    }

    // ------------------------------------------------
    // 2) Load the game
    // ------------------------------------------------
    public void LoadGame()
    {
        // Load from JSON via SaveSystem
        SaveData data = SaveSystem.Load();

        // If no data is found, do nothing
        if (data == null)
        {
            Debug.LogWarning("No data to load or failed to load data.");
            return;
        }

        // Make sure PlayerData is available
        if (PlayerData.instance == null)
        {
            Debug.LogError("PlayerData instance is null! Cannot load.");
            return;
        }

        // Apply the data back to PlayerData (again, no changes within PlayerData)
        PlayerData.instance.playerHealth = data.playerHealth;
        PlayerData.instance.keysCollected = data.keysCollected;
        PlayerData.instance.objective = data.objective;
        PlayerData.instance.difficulty = data.difficulty;

        // Clear the existing inventory
        PlayerData.instance.inventory.Clear();

        // Rebuild the inventory from item names
        foreach (string itemName in data.inventoryItemNames)
        {
            // Attempt to load a sprite from Resources based on the item name
            // Example: itemName = "Attack Card" => we load "Attack"
            // You can adapt this to suit your naming scheme.
            // If your Resources folder has an "Attack.png" sprite, for instance:
            string resourceName = itemName.Replace(" Card", "");
            Sprite sprite = Resources.Load<Sprite>(resourceName);

            // Reuse the existing AddItem method from PlayerData
            // (We pass a simple description since we don't store it in SaveData.)
            PlayerData.instance.AddItem(itemName, sprite, "Restored item from save.");
        }

        // Now that PlayerData fields are updated, refresh the UI
        // You can call any public method from PlayerData to update displays
        PlayerData.instance.UpdateKeyDisplay();

        // For health:
        // You can either call PlayerData.instance.SetPlayerHealth( ... ) 
        // or just call the method that updates text:
        // (We do this if we want to ensure the UI updates immediately)
        // But it's optional if your game updates the UI automatically when changing the field
        // For safety:
        PlayerData.instance.SetPlayerHealth(data.playerHealth);

        // Also update the objective text:
        PlayerData.instance.setObjectiveText(data.objective);

        Debug.Log("Game data loaded and applied to PlayerData!");
    }
}
