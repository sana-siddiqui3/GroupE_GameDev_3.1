using UnityEngine;

public class Card : MonoBehaviour
{
    public string cardName;  // Name or type of the card
    public Sprite cardImage;  // Image or icon for UI purposes

    // When the player picks up the card
    public void PickUp()
    {
        PlayerInventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
        playerInventory.AddCard(this);  // Add card to player's inventory
        Destroy(gameObject);  // Destroy the card in the scene
    }

    // Optional: display some info or prompt when near the card
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Press 'E' to pick up " + cardName);
        }
    }
}
