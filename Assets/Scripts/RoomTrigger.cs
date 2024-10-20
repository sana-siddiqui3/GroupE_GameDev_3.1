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

        private PlayerInventory inventory;
        private bool playerInRange = false;

        void Start()
        {
            roomEnterPromptUI.SetActive(false);
            NotEnoughKeysUI.SetActive(false);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = true;
                inventory = player.GetComponent < PlayerInventory > ();

                int keys = inventory.getKeys();

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

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = false;
                roomEnterPromptUI.SetActive(false);
                NotEnoughKeysUI.SetActive(false);
            }
        }

        void Update()
        {
            if (playerInRange && Input.GetKeyDown(KeyCode.E))
            {
                enterRoom();
            }
        }

        void enterRoom()
        {
            Debug.Log("entered new room");

            roomEnterPromptUI.SetActive(false);

            RoomChangeUI.SetActive(true);
        }
    }
}