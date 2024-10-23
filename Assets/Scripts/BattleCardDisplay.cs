using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BattleCardDisplay : MonoBehaviour
{
    public GameObject cardSlotPrefab;  
    public Transform cardPanel;        

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
            
            GameObject newCardSlot = Instantiate(cardSlotPrefab, cardPanel);

            Text cardText = newCardSlot.GetComponentInChildren<Text>(); 
            if (cardText != null)
            {
                cardText.text = cardName;
            }

            
            cardSlots.Add(newCardSlot);
        }

        isCardDisplayUpdated = true;
    }
}
