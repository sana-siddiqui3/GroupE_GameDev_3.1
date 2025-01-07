using UnityEngine;
using System.Collections;
using TMPro;

namespace Assets.Scripts
{
    public class BridgeReward : MonoBehaviour
    {
        private bool playerInRange = false;
        private bool hasBeenRewarded = false;  // Flag to ensure player is rewarded only once
        private PlayerData inventory;
        public TextMeshProUGUI inventoryNotificationText;

        void Start()
        {
            // Ensure PlayerData.instance is initialized properly
            inventory = PlayerData.instance;

            if (inventory == null)
            {
                Debug.LogError("PlayerData instance is not initialized.");
            }
            else
            {
                Debug.Log("PlayerData instance found.");
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = true;
                Debug.Log("Player entered the trigger area.");
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = false;
                Debug.Log("Player exited the trigger area.");
            }
        }

        void Update()
        {
            if (playerInRange && !hasBeenRewarded)
            {
                // Reward the player with items when they reach the platform, only once
                RewardPlayer();
                hasBeenRewarded = true;  // Set flag to true to prevent further rewards
            }
        }

        void RewardPlayer()
        {
            Debug.Log("Player reached the end of the glass bridge, rewarding with items!");

            // Check if PlayerData instance is valid before adding items
            if (inventory != null)
            {
                // Add items to inventory
                inventory.AddItem("Poison Potion", Resources.Load<Sprite>("PoisonPotion"), "A potion that allows you to skip a battle of choice.", false);
                inventory.AddItem("Health Potion", Resources.Load<Sprite>("HealthPotion"), "A potion that restores health.", false);
                inventory.AddItem("AttackBlock Card", Resources.Load<Sprite>("AttackBlock"), "A card that attacks & blocks.", false);

                // Show a notification
                string itemsAddedMessage = "Added to inventory:\n";
                itemsAddedMessage += "• Poison Potion\n";
                itemsAddedMessage += "• Health Potion\n";
                itemsAddedMessage += "• AttackBlock Card\n";

                inventoryNotificationText.text = itemsAddedMessage;

                // Optionally hide the notification after a delay
                StartCoroutine(HideInventoryNotification());
            }

        }

        private IEnumerator HideInventoryNotification()
        {
            yield return new WaitForSeconds(3f); // Show the message for 3 seconds
            inventoryNotificationText.text = ""; // Clear the text
        }
    }
}
