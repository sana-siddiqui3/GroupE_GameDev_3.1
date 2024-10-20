using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<string> inventory = new List<string>();  // List to hold player's items

    // Add an item to the inventory
    public void AddItem(string item)
    {
        inventory.Add(item);
        Debug.Log("Added " + item + " to inventory.");
    }

    // Check if an item is in the inventory
    public bool HasItem(string item)
    {
        return inventory.Contains(item);
    }

    // Print all items in the inventory
    public void PrintInventory()
    {
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

}
