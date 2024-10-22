using UnityEngine;

public class Chest : MonoBehaviour
{
    public GameObject KeyPromptUI;

    private bool isOpen = false;   // State of the chest
    private bool playerInRange = false;  // Check if the player is close
    private bool empty = false;

    void Start()
    {
        KeyPromptUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!isOpen)
            {
                isOpen = true;
                GiveKeyItem();
            }
        }
    }

    private void GiveKeyItem()
    {
        // Get the PlayerInventory component from the player
        //PlayerInventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();

        // Check if the key has already been collected
        if (!empty)
        {
            PlayerData.instance.AddKey();
            Debug.Log("Player received: 1 Key");
            empty = true;
        }
        else
        {
            Debug.Log("This chest is empty.");
        }
        isOpen = false;
    }

    // Detect if the player is in range to interact with the chest
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            KeyPromptUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        playerInRange = false;
        KeyPromptUI.SetActive(false);
    }
}
