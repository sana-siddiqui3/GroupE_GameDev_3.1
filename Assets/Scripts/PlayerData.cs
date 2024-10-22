using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData instance;

    // Player's inventory and key count
    public List<string> cardInventory = new List<string>();
    public int keysCollected = 0;
    public int totalKeysRequired = 2;

    private void Awake()
    {
        // Ensure this instance persists across scenes
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Prevent this object from being destroyed when loading new scenes
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicate instances
        }
    }

    // Method to add a card to the inventory
    public void AddCard(string card)
    {
        cardInventory.Add(card);
        PlayerInventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();  // Find PlayerInventory in the scene
        if (playerInventory != null)
        {
            playerInventory.UpdateInventoryDisplay();  // Trigger UI update
        }
    }

    // Method to add a key
    public void AddKey()
    {
        keysCollected++;
        PlayerInventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();  // Find PlayerInventory in the scene
        if (playerInventory != null)
        {
            playerInventory.UpdateInventoryDisplay();  // Trigger UI update
        }
    }

    // Method to retrieve the number of keys
    public int GetKeys()
    {
        return keysCollected;
    }
}
