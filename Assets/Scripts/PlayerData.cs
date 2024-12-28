using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData instance;
    public float playerHealth = 100f;

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

    private void Start()
    {
        // Initialize the player's card inventory with 10 starting cards
        InitializeStartingCards();
    }

    // Method to initialize the starting cards (10 cards)
    private void InitializeStartingCards()
    {
        if (cardInventory == null)
        {
            cardInventory = new List<string>();
        }

        // Add 10 predefined cards to the cardInventory
        cardInventory.Add("Attack");
        cardInventory.Add("Attack");
        cardInventory.Add("Attack");
        cardInventory.Add("Heal");
        cardInventory.Add("Heal");
        cardInventory.Add("Attack");
        cardInventory.Add("Attack");
        cardInventory.Add("Attack");
        cardInventory.Add("Heal");
        cardInventory.Add("Heal");

        Debug.Log("Starting cards initialized: " + string.Join(", ", cardInventory));
    }

    public void SavePlayerHealth(float health)
    {
        playerHealth = Mathf.Clamp(health, 0, 100); // Ensure health stays between 0 and 100
    }

    // Method to add a card to the inventory
    public void AddCard(string card)
    {
        cardInventory.Add(card);
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

    // Method to remove a card from the inventory
    public void RemoveCard(string card)
    {
        if (cardInventory.Contains(card))
        {
            cardInventory.Remove(card);
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

    // Method to add a key
    public void AddKey()
    {
        keysCollected++;
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

    // Method to retrieve the number of keys
    public int GetKeys()
    {
        return keysCollected;
    }
}
