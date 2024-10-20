using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public List<string> inventory = new List<string>();  // List to hold player's items
    int keys = 0;
    int keysRequired = 2;
    public Text keyCountText;  // Reference to the HUD text element

    // Add an item to the inventory
    public void AddItem(string item)
    {
        inventory.Add(item);
        Debug.Log("Added " + item + " to inventory.");
    }

    // Add a key to the inventory
    public void addKey() { 
        keys++;
        UpdateKeyCountHUD();
    }

    // Remove n keys from inventory
    public void removeKeys(int n) { keys -= n; }

    // Return number of keys
    public int getKeys() { return keys; }

    // Check if an item is in the inventory
    public bool HasItem(string item)
    {
        return inventory.Contains(item);
    }

    // Print all items in the inventory
    public void PrintInventory()
    {
        if (keys > 0)
            Debug.Log("Keys: " + keys);
        Debug.Log("Inventory contains:");
        foreach (var i in inventory)
        {
            Debug.Log(i);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))  // Press 'I' to view inventory
        {
            PrintInventory();
        }
    }

    private void UpdateKeyCountHUD()
    {
        if (keyCountText != null)
        {
            keyCountText.text = "Keys: " + keys + "/" + keysRequired;
        }
    }

}
