using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BattleCardDisplay : MonoBehaviour
{
    public GameObject cardSlotPrefab;  // Prefab of the UI element that represents a card slot
    public Transform cardPanel;        // The parent UI element (Panel) where card slots will be placed

    private List<GameObject> cardSlots = new List<GameObject>();
    private bool isCardDisplayUpdated = false;  // Flag to check if cards are displayed

    void Start()
    {
        UpdateCardDisplay();  // Update the display when the battle starts
    }

    public void UpdateCardDisplay()
    {
        if (isCardDisplayUpdated) return; // Prevent updating if already done

        // Clear previous card slots
        foreach (GameObject slot in cardSlots)
        {
            Destroy(slot);
        }
        cardSlots.Clear();

        // Get the player's card inventory
        List<string> playerCards = PlayerData.instance.cardInventory;

        // Loop through the card inventory and create a UI slot for each card
        foreach (string cardName in playerCards)
        {
            // Instantiate a new card slot from the prefab
            GameObject newCardSlot = Instantiate(cardSlotPrefab, cardPanel);

            // Set the card name
            Text cardText = newCardSlot.GetComponentInChildren<Text>(); 
            if (cardText != null)
            {
                cardText.text = cardName;  // Set the card's name in the slot
            }

            // Add to the list of card slots for management
            cardSlots.Add(newCardSlot);
        }

        isCardDisplayUpdated = true; // Set the flag to true after updating
    }
}
