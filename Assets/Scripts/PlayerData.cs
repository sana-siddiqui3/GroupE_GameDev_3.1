using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerData : MonoBehaviour
{
    public static PlayerData instance;
    public float playerHealth = 100f;

        // Player's inventory and key count
    public List<InventoryItem> inventory = new List<InventoryItem>();
    public int keysCollected = 0;
    public int totalKeysRequired = 2;
    public string objective = "";

    public TextMeshProUGUI objectiveText;

    private void Awake()
    {
        // Ensure this instance persists across scenes
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

    private void Start()
    {
        // Initialize the player's inventory
        InitializeStartingItems();
        setObjective("Get the keys to unlock the door");
    }

    // Method to initialize starting inventory items
    private void InitializeStartingItems()
    {
        // Example: Add predefined items
        AddItem("Attack Card", Resources.Load<Sprite>("Attack"), "A basic attack card.");
        AddItem("Heal Card", Resources.Load<Sprite>("Heal"), "A basic healing card.");
    }

    // Method to add an item to the inventory
    public void AddItem(string name, Sprite sprite, string description = "")
    {
        InventoryItem newItem = new InventoryItem
        {
            itemName = name,
            itemSprite = sprite,
            itemDescription = description
        };

        inventory.Add(newItem);
        if(name == "Key")
        {
            keysCollected++;
            if(keysCollected == totalKeysRequired)
            {
                setObjective("Unlock the door to escape!");
            }
        }
        Debug.Log($"Added {name} to inventory!");

        // Update the inventory display if needed
        PlayerInventory playerInventory = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerInventory>();
        if (playerInventory != null)
        {
            playerInventory.UpdateInventoryDisplay();
        }
        else
        {
            Debug.LogError("PlayerInventory not found!");
        }
    }

    // Method to remove an item from the inventory
    public void RemoveItem(string name)
    {
        InventoryItem itemToRemove = inventory.Find(item => item.itemName == name);
        if (itemToRemove != null)
        {
            inventory.Remove(itemToRemove);
            Debug.Log($"Removed {name} from inventory!");

            // Update the inventory display if needed
            PlayerInventory playerInventory = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerInventory>();
            if (playerInventory != null)
            {
                playerInventory.UpdateInventoryDisplay();
            }
            else
            {
                Debug.LogError("PlayerInventory not found!");
            }
        }
    }

    public void SavePlayerHealth(float health)
    {
        playerHealth = Mathf.Clamp(health, 0, 100); // Ensure health stays between 0 and 100
    }

    // Method to retrieve the number of keys
    public int GetKeys()
    {
        return keysCollected;
    }

    // Method to set the objective
    public void setObjective(string obj)
    {
        objective = obj;
        setObjectiveText(objective);
    }

    // Method to retrieve the objective
    public string getObjective()
    {
        return objective;
    }

    // Method to set the objective text
    public void setObjectiveText(string text)
    {
        objectiveText.text = text;
    }
}
