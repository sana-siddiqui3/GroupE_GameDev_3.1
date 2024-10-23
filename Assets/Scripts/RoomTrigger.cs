using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class RoomTrigger : MonoBehaviour
    {
        public GameObject roomEnterPromptUI;
        public GameObject RoomChangeUI;
        public GameObject player;
        public GameObject NotEnoughKeysUI;

        private int keys;
        private PlayerData inventory;
        private bool playerInRange = false;

        void Start()
        {
            roomEnterPromptUI.SetActive(false);  // Hide the room enter prompt at start
            NotEnoughKeysUI.SetActive(false);  // Hide the "not enough keys" UI at start
        }

        // Detect when the player enters the trigger area
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = true;
                inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<PlayerData>();

                keys = inventory.GetKeys();

                // Show appropriate UI based on key count
                if (keys == 2)
                {
                    roomEnterPromptUI.SetActive(true);
                }
                else
                {
                    NotEnoughKeysUI.SetActive(true);
                }
            }
        }

        // Detect when the player exits the trigger area
        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = false;
                roomEnterPromptUI.SetActive(false); 
                NotEnoughKeysUI.SetActive(false); 
            }
        }

        // Check for player input to enter the room
        void Update()
        {
            if (playerInRange && Input.GetKeyDown(KeyCode.E) && keys == 2)
            {
                enterRoom(); 
            }
        }

        // Handles the logic for entering the room
        void enterRoom()
        {
            Debug.Log("entered new room");  

            roomEnterPromptUI.SetActive(false);  
            RoomChangeUI.SetActive(true);  
        }
    }
}
