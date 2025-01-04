using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
    public class RoomTrigger : MonoBehaviour
    {
        public GameObject roomEnterPromptUI;
        public GameObject RoomChangeUI;
        public GameObject player;
        public GameObject NotEnoughItemsUI;

        private PlayerData inventory;
        private bool playerInRange = false;

        // Reference to the input action for interaction
        private PlayerInput controls;
        private InputAction interactAction;

        // Enum to indicate which room the trigger corresponds to
        public enum RoomType { Room1, Room2, Room3 }
        public RoomType currentRoom;

        private GameControllerRoom2 gameController;  // Reference to Room 2 game controller

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
            NotEnoughItemsUI.SetActive(false);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = true;
                inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<PlayerData>();

                // Room-specific checks
                if (currentRoom == RoomType.Room1)
                {
                    // Check if the player has 2 keys for Room 1
                    int keys = inventory.GetKeys();
                    if (keys >= 2)
                    {
                        roomEnterPromptUI.SetActive(true);
                    }
                    else
                    {
                        NotEnoughItemsUI.SetActive(true);
                    }
                }
                else if (currentRoom == RoomType.Room2)
                {
                    gameController = GameObject.FindGameObjectWithTag("GameControllerRoom2")?.GetComponent<GameControllerRoom2>();
                    // Check if all ghosts are defeated for Room 2
                    if (gameController.IsVictoryAchieved())
                    {
                        roomEnterPromptUI.SetActive(true); // Show enter prompt
                    }
                    else
                    {
                        NotEnoughItemsUI.SetActive(true); // Show seal is not broken message
                    }
                }
                else if (currentRoom == RoomType.Room3)
                {
                    // Check if the player has the "Purified Heart" for Room 3
                    bool hasPurifiedHeart = inventory.HasItem("Purified Heart");
                    if (hasPurifiedHeart)
                    {
                        roomEnterPromptUI.SetActive(true);
                    }
                    else
                    {
                        NotEnoughItemsUI.SetActive(true);
                    }
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = false;
                roomEnterPromptUI.SetActive(false);
                NotEnoughItemsUI.SetActive(false);
            }
        }

        void Update()
        {
            if (playerInRange && interactAction.triggered)
            {
                // Room-specific enter logic
                if (currentRoom == RoomType.Room1 && inventory.GetKeys() >= 2)
                {
                    PlayerData.instance.RemoveItem("Key");
                    PlayerData.instance.RemoveItem("Key");
                    EnterRoom();
                }
                else if (currentRoom == RoomType.Room2 && gameController.IsVictoryAchieved())
                {
                    EnterRoom();
                }
                else if (currentRoom == RoomType.Room3 && inventory.HasItem("Purified Heart"))
                {
                    EnterRoom();
                }
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
