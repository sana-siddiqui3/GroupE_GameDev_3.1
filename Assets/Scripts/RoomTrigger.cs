using UnityEngine;
using UnityEngine.InputSystem;

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

        // Reference to the input action for interaction
        private PlayerInput controls;
        private InputAction interactAction;

        void Awake()
        {
            controls = new PlayerInput();
            interactAction = controls.Player.Interact; // Assuming the action is named Interact
        }

        void OnEnable()
        {
            controls.Enable(); // Enable the action map
        }

        void OnDisable()
        {
            controls.Disable(); // Disable the action map
        }

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
                inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<PlayerData>();

                keys = inventory.GetKeys();

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
            if (playerInRange && interactAction.triggered && keys == 2)
            {
                EnterRoom();
            }
        }

        void EnterRoom()
        {
            Debug.Log("Entered new room");

            roomEnterPromptUI.SetActive(false);
            RoomChangeUI.SetActive(true);
        }
    }
}
