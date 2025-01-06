using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class RandomChest : MonoBehaviour
{
    private bool isOpen = false;
    private bool playerInRange = false;
    private bool empty = false;
    public TextMeshProUGUI chestMessageText;

    // List of items that the chest can give
    private List<Item> items = new List<Item>();

    void Start()
    {
        chestMessageText.text = "";
        
        items.Add(new Item("Health Potion", Resources.Load<Sprite>("HealthPotion"), "Restores 20 health."));
        items.Add(new Item("Attack Card", Resources.Load<Sprite>("Attack"), "A basic attack card."));
        items.Add(new Item("Heal Card", Resources.Load<Sprite>("Heal"), "A basic healing card."));
        items.Add(new Item("Energy Card", Resources.Load<Sprite>("Energy"), "A card that restores energy."));
        items.Add(new Item("Shield Card", Resources.Load<Sprite>("Shield"), "A basic shield card."));
        items.Add(new Item("AttackBlock Card", Resources.Load<Sprite>("AttackBlock"), "A card that attacks & blocks."));
        items.Add(new Item("TripleAttack Card", Resources.Load<Sprite>("TripleAttack"), "A card that performs 3 attacks."));
        items.Add(new Item("AttackAll Card", Resources.Load<Sprite>("AttackAll"), "A card that attacks all enemies."));
        items.Add(new Item("BadAttack Card", Resources.Load<Sprite>("BadAttack"), "An inefficient attack card."));
        items.Add(new Item("LowAttack Card", Resources.Load<Sprite>("LowAttack"), "A low damage attack card."));
    
    }

    void Update()
    {
        if (playerInRange && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (!isOpen)
            {
                isOpen = true;
                GiveRandomItem();
            }
        }
    }

    private void GiveRandomItem()
    {
        if (!empty)
        {
            // Select a random item from the list
            Item randomItem = items[Random.Range(0, items.Count)];

            // Add the random item to the player's inventory
            PlayerData.instance.AddItem(randomItem.Name, randomItem.Icon, randomItem.Description);
            
            // Display a message to the player
            chestMessageText.text = "Added " + randomItem.Name + ".";
            empty = true;
        }
        else
        {
            chestMessageText.text = "This item is empty.";
        }
        isOpen = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered trigger with: " + other.gameObject.name);
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            chestMessageText.text = "Press 'E' to open";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exited trigger with: " + other.gameObject.name);
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            chestMessageText.text = "";
        }
    }
}

// Item class to represent each item
public class Item
{
    public string Name;
    public Sprite Icon;
    public string Description;

    public Item(string name, Sprite icon, string description)
    {
        Name = name;
        Icon = icon;
        Description = description;
    }
}
