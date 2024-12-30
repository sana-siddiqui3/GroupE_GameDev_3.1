using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem; // Add this line for the new input system
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    public TextMeshProUGUI keyCountText;
    public TextMeshProUGUI inventoryDisplay;

    private void Start()
    {
  
        UpdateInventoryDisplay();
        
    }

    public void UpdateInventoryDisplay()
    {
        if (PlayerData.instance == null)
        {
            Debug.LogError("PlayerData.instance is still null!");
            return;
        }

        // Update the UI elements with the player's current inventory and keys
        keyCountText.text = "Keys: " + PlayerData.instance.keysCollected + "/" + PlayerData.instance.totalKeysRequired;
        inventoryDisplay.text = "Inventory:\n";

        // Check if card inventory is initialized
        if (PlayerData.instance.cardInventory == null)
        {
            Debug.LogError("PlayerData.instance.cardInventory is null!");
            return;
        }

        foreach (string card in PlayerData.instance.cardInventory)
        {
            inventoryDisplay.text += card + "\n";
        }
    }
}
