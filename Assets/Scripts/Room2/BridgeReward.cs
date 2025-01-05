using UnityEngine;

namespace Assets.Scripts
{
    public class BridgeReward : MonoBehaviour
    {
        private bool playerInRange = false;
        private bool hasBeenRewarded = false;  // Flag to ensure player is rewarded only once
        private PlayerData inventory;

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
                inventory.AddItem("Poison Potion", Resources.Load<Sprite>("PoisonPotion"), "A potion that inflicts poison.");
                inventory.AddItem("Health Potion", Resources.Load<Sprite>("HealthPotion"), "A potion that restores health.");
                inventory.AddItem("Health Potion", Resources.Load<Sprite>("HealthPotion"), "A potion that restores health.");
                inventory.AddItem("AttackBlock Card", Resources.Load<Sprite>("AttackBlock"), "A card that attacks & blocks.");
                Debug.Log("Items have been added to the player's inventory.");
            }
            else
            {
                Debug.LogError("PlayerData instance is invalid. Items could not be added.");
            }
        }
    }
}
