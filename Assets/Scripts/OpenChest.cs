using UnityEngine;
using TMPro;  

public class Chest : MonoBehaviour
{
    public EnemyTrigger enemyTrigger; 

    private bool isOpen = false;   
    private bool playerInRange = false;  
    private bool empty = false;
    public TextMeshProUGUI chestMessageText;

    void Start()
    {
        chestMessageText.text = "";  // Initialize the message text to be empty
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E)) 
        {
            if (!isOpen && enemyTrigger.isEnemyDefeated)  // Chest can be opened if the enemy is defeated
            {
                isOpen = true;
                GiveKeyItem();  
            }
            else if (!enemyTrigger.isEnemyDefeated)  
            {
                chestMessageText.text = "Defeat the enemy first.";  
            }
        }
    }

    private void GiveKeyItem()
    {
        // Check if the key has already been collected
        if (!empty)
        {
            PlayerData.instance.AddKey();  
            chestMessageText.text = "Added key.";  
            empty = true;
        }
        else
        {
            chestMessageText.text = "This chest is empty.";  
        }
        isOpen = false;
    }

    // Detect if the player is in range to interact with the chest
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  
        {
            playerInRange = true;
            chestMessageText.text = "Press 'E' to open chest";  
        }
    }

    // Detect if the player exits the chest's range
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            chestMessageText.text = "";  
        }
    }
}
