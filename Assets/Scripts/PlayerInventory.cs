using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public List<string> inventory = new List<string>();  // List to hold player's items
    private int keys = 0;
    private int keysRequired = 2;
    public List<string> cardInventory = new List<string>();  
    public Text keyCountText;
    public Text inventoryDisplay;

    private RectTransform inventoryDisplayRectTransform;  // Reference to the RectTransform of the inventory display
    public float lineHeight = 24f;  // The height to increase per item, adjust based on font size

    private void Start()
    {
        // Get the RectTransform of the inventoryDisplay
        inventoryDisplayRectTransform = inventoryDisplay.GetComponent<RectTransform>();

        UpdateInventoryDisplay();  // Manually trigger the inventory display update at the start
    }

    // Add an item to the inventory (for non-card items)
    public void AddItem(string item)
    {
        inventory.Add(item);
        Debug.Log("Added " + item + " to inventory.");
        AdjustInventoryDisplayHeight(); // Adjust the height of the RectTransform to accommodate the new item
        UpdateInventoryDisplay();
    }

    // Add a key to the inventory
    public void AddKey() 
    { 
        keys++;
        Debug.Log("Added 1 Key. Current keys: " + keys);
        UpdateInventoryDisplay();
    }

    // Remove n keys from inventory
    public void RemoveKeys(int n) 
    { 
        keys -= n; 
        Debug.Log("Removed " + n + " keys.");
    }

    // Return number of keys
    public int GetKeys() 
    { 
        return keys; 
    }

    // Add a card to the inventory
    public void AddCard(string card)
    {
        cardInventory.Add(card);
        Debug.Log("Picked up card: " + card);
        AdjustInventoryDisplayHeight(); // Adjust the height of the RectTransform to accommodate the new item
        UpdateInventoryDisplay();
    }

    // Print all items (keys and cards) in the inventory
    public void PrintInventory()
    {
        Debug.Log("Inventory contains:");

        // Display cards
        if (cardInventory.Count > 0)
        {
            Debug.Log("Cards:");
            foreach (string card in cardInventory)
            {
                Debug.Log(" - " + card);
            }
        }
        else
        {
            Debug.Log("No cards in the inventory.");
        }

        // Display keys
        Debug.Log("Keys: " + keys + "/" + keysRequired);
    }

    void Update()
    {
        // Press 'I' to view the inventory
        if (Input.GetKeyDown(KeyCode.I))  
        {
            PrintInventory();
        }
    }

    // Update key count in the HUD (optional UI)
    /*private void UpdateKeyCountHUD()
    {
        if (keyCountText != null && PlayerData.instance != null)
        {
            
        }
    }*/

    // Update Inventory display for HUD
    public void UpdateInventoryDisplay()
    {
        if (keyCountText != null && inventoryDisplay != null && PlayerData.instance != null)
        {
            keyCountText.text = "Keys: " + PlayerData.instance.keysCollected + "/" + PlayerData.instance.totalKeysRequired;
            inventoryDisplay.text = "Inventory:\n";

            // Loop through each item in the inventory and add it to the display
            //foreach (string item in inventory)
            //{
            //    inventoryDisplay.text += item + "\n";
            //}

            foreach (string card in PlayerData.instance.cardInventory)
            {
                inventoryDisplay.text += card + "\n";
            }
        }
    }

    // Adjust the height of the RectTransform to accommodate more text
    private void AdjustInventoryDisplayHeight()
    {
        // Calculate the new height based on the number of cards in the inventory
        float newHeight = PlayerData.instance.cardInventory.Count * lineHeight;

        // Set the new height to the RectTransform
        inventoryDisplayRectTransform.sizeDelta = new Vector2(inventoryDisplayRectTransform.sizeDelta.x, newHeight);
    }
}
