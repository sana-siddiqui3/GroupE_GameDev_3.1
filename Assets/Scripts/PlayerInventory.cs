using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem; // Add this line for the new input system

public class PlayerInventory : MonoBehaviour
{
    public Text keyCountText;
    public Text inventoryDisplay;
    private RectTransform inventoryDisplayRectTransform;

    private void Start()
    {
        inventoryDisplayRectTransform = inventoryDisplay.GetComponent<RectTransform>();
        UpdateInventoryDisplay();
    }

    public void UpdateInventoryDisplay()
    {
        {
            keyCountText.text = "Keys: " + PlayerData.instance.keysCollected + "/" + PlayerData.instance.totalKeysRequired;
            inventoryDisplay.text = "Inventory:\n";

            foreach (string card in PlayerData.instance.cardInventory)
            {
                inventoryDisplay.text += card + "\n";
            }
        }
    }
}
