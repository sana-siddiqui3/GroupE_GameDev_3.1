using UnityEngine;
using TMPro;
using UnityEngine.InputSystem; // Add this line for the new input system

public class OpenKeyChest : MonoBehaviour
{
    public EnemyController enemyTrigger;

    private bool isOpen = false;
    private bool playerInRange = false;
    private bool empty = false;
    public TextMeshProUGUI chestMessageText;

    void Start()
    {
        chestMessageText.text = "";
    }

    void Update()
    {
        if (playerInRange && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (!isOpen && enemyTrigger.isEnemyDefeated)
            {
                isOpen = true;
                chestMessageText.text = "";
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
        if (!empty)
        {
            PlayerData.instance.AddItem("Key", Resources.Load<Sprite>("Key"), "A shiny key to unlock doors.");
            empty = true;
        }
        else
        {
            chestMessageText.text = "This chest is empty.";
        }
        isOpen = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            chestMessageText.text = "Press 'E' to open chest";
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            chestMessageText.text = "";
        }
    }
}
