using UnityEngine;

public class Card : MonoBehaviour
{
    public string cardName; 
    public Sprite cardImage;  // Image or icon for UI purposes

    private bool playerInRange = false;  // To track if the player is near the card

    void Update()
    {
        // Check if the player is in range and presses the "E" key
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            PickUp();
        }
    }

    // When the player picks up the card
    public void PickUp()
    {
        PlayerInventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
        playerInventory.AddCard(this);  // Add card to player's inventory
        Destroy(gameObject);  // Destroy the card in the scene
    }

    // Trigger when the player enters the card's pickup area
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;  // Player is in range to pick up the card
            Debug.Log("Press 'E' to pick up " + cardName);
        }
    }

    // Trigger when the player exits the card's pickup area
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false; 
        }
    }
}
