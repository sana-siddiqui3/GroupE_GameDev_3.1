using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerData : MonoBehaviour
{
    public static PlayerData instance;
    public float playerHealth = 100f;
    public List<InventoryItem> inventory = new List<InventoryItem>();
    public int keysCollected = 0;
    public int totalKeysRequired = 2;
    public string objective = "";

    public TextMeshProUGUI objectiveText;
    public TextMeshProUGUI healthDisplay;
    public TextMeshProUGUI keyDisplay;

    public int difficulty;

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
        difficulty = PlayerPrefs.GetInt("Difficulty", 1); // Default: 1
        UpdateHealthDisplay();
        setObjective("Fight the enemy to access the chest.");
        InitializeStartingItems();
    }

    // Method to initialize starting inventory items
    private void InitializeStartingItems()
    {
        // Example: Add predefined items
        AddItem("Attack Card", Resources.Load<Sprite>("Attack"), "A basic attack card.");
        AddItem("Attack Card", Resources.Load<Sprite>("Attack"), "A basic attack card.");
        AddItem("Attack Card", Resources.Load<Sprite>("Attack"), "A basic attack card.");
        AddItem("Attack Card", Resources.Load<Sprite>("Attack"), "A basic attack card.");
        AddItem("Heal Card", Resources.Load<Sprite>("Heal"), "A basic healing card.");
        AddItem("Poison Potion", Resources.Load<Sprite>("PoisonPotion"), "A potion that poisons the enemy.");
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
            UpdateKeyDisplay();
            if(keysCollected == totalKeysRequired)
            {
                setObjective("Unlock the door to escape!");
            } else {
                setObjective("Find the other key to unlock the door.");
            }
        }
        Debug.Log($"Added {name} to inventory!");

        // Update the inventory display
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
            if(name == "Key")
            {
                keysCollected--;
                UpdateKeyDisplay();
            }
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

    // Method to heal the player
    public void HealPlayer(float amount)
    {
        playerHealth = Mathf.Clamp(playerHealth + amount, 0, 100); // Heal but cap at max health
        Debug.Log($"Player healed by {amount}. Current health: {playerHealth}");
        UpdateHealthDisplay();
    }

    // Method to damage the player
    public void DamagePlayer(float amount)
    {
        playerHealth = Mathf.Clamp(playerHealth - amount, 0, 100); // Take damage but not below 0
        Debug.Log($"Player took {amount} damage. Current health: {playerHealth}");
        UpdateHealthDisplay();
    }

    // Method to update the health display
    private void UpdateHealthDisplay()
    {
        if (healthDisplay != null)
        {
            healthDisplay.text = $"Health: {playerHealth}";
        }
        else
        {
            Debug.LogWarning("Health display text is not assigned!");
        }
    }

    // Method to update the key display
    public void UpdateKeyDisplay()
    {
        if (keyDisplay != null)
        {
            keyDisplay.text = $"Keys: {keysCollected}/{totalKeysRequired}";
        }
        else
        {
            Debug.LogWarning("Key display text is not assigned!");
        }
    }

    public bool HasItem(string itemName)
    {
        // Check if any item in the inventory matches the given itemName
        foreach (var item in inventory)
        {
            if (item.itemName == itemName)
            {
                return true;
            }
        }
        return false;
    }
}
