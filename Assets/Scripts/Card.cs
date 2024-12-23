using UnityEngine;
using TMPro;

public class Card : MonoBehaviour
{
    public string cardName;
    
    private bool playerInRange = false;  // Tracks if the player is near the card

    public GameObject cardUIPrefab; 
    public GameObject cardPanel; 

    public TextMeshProUGUI pickupMessageText;

    void Update()
    {
        // Check if the player is in range and presses "E" to pick up the card
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            PickUp();
        }
    }

    // Adds the card to the player's inventory and updates the UI
    public void PickUp()
    {
        PlayerData.instance.AddCard(cardName);

        GameObject cardUI = Instantiate(cardUIPrefab, cardPanel.transform);

        CardUI cardUIScript = cardUI.GetComponent<CardUI>();
        cardUIScript.SetCardName(cardName);

        pickupMessageText.text = ""; // Clears the pickup message

        Destroy(gameObject); // Removes the card from the scene
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            pickupMessageText.text = "Press 'E' to pick up " + cardName;  // Displays the pickup prompt
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            pickupMessageText.text = "";  // Clears the message when the player leaves
        }
    }
}
