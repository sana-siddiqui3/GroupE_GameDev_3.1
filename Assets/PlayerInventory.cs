using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public List<string> inventory = new List<string>();  // List to hold player's items
    private int keys = 0;
    private int keysRequired = 2;
    public List<Card> cardInventory = new List<Card>();  // List to hold player's cards
    public Text keyCountText;  // Reference to the HUD text element

    // Add an item to the inventory (for non-card items)
    public void AddItem(string item)
    {
        inventory.Add(item);
        Debug.Log("Added " + item + " to inventory.");
    }

    // Add a key to the inventory
    public void addKey() 
    { 
        keys++;
        UpdateKeyCountHUD();
        Debug.Log("Added 1 Key. Current keys: " + keys);
    }

    // Remove n keys from inventory
    public void removeKeys(int n) 
    { 
        keys -= n; 
        Debug.Log("Removed " + n + " keys.");
    }

    // Return number of keys
    public int getKeys() 
    { 
        return keys; 
    }

    // Add a card to the inventory
    public void AddCard(Card card)
    {
        cardInventory.Add(card);
        Debug.Log("Picked up card: " + card.cardName);
    }

    // Print all items (keys and cards) in the inventory
    public void PrintInventory()
    {
        Debug.Log("Inventory contains:");

        // Display cards
        if (cardInventory.Count > 0)
        {
            Debug.Log("Cards:");
            foreach (Card card in cardInventory)
            {
                Debug.Log(" - " + card.cardName);
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
    private void UpdateKeyCountHUD()
    {
        if (keyCountText != null)
        {
            keyCountText.text = "Keys: " + keys + "/" + keysRequired;
        }
    }
}
