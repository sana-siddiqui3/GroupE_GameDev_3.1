using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class CardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI cardNameText; // The card's name text
    public GameObject hoverInfoPanel; // The UI panel that shows additional info
    public TextMeshProUGUI hoverInfoText; // The TextMeshPro component to show info

    private string cardName;
    private int energyCost;
    private int attackAmount;

    // Set the card details (card name, energy cost, attack value)
    public void SetCardDetails(string cardName, int energyCost, int attackAmount)
    {
        this.cardName = cardName;
        this.energyCost = energyCost;
        this.attackAmount = attackAmount;
        cardNameText.text = cardName; // Update the name of the card in the UI
    }

    // When the mouse enters the card, show the hover info
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverInfoPanel != null)
        {
            hoverInfoPanel.SetActive(true); // Show the hover info panel
            hoverInfoText.text = GetHoverTextForCard(cardName, attackAmount); // Set the hover text based on the card
            hoverInfoText.text += $"\nEnergy Cost: {energyCost}"; // Add the energy cost to the hover text
        }
    }

    // When the mouse exits the card, hide the hover info
    public void OnPointerExit(PointerEventData eventData)
    {
        if (hoverInfoPanel != null)
        {
            hoverInfoPanel.SetActive(false); // Hide the hover info panel
        }
    }

    // Get custom hover text based on the card type
    private string GetHoverTextForCard(string cardName, int attackAmount)
    {
        switch (cardName)
        {
            case "Attack Card":
                return $"This card will attack {attackAmount} damage.";
            case "Heal Card":
                return $"This card will heal {attackAmount} health.";
            case "Shield Card":
                return $"This card will shield for {attackAmount} health.";
            case "Energy Card":
                return $"This card will restore {attackAmount} energy.";
            case "AttackBlock Card":
                return $"This card will attack {attackAmount} damage and heal {attackAmount} health.";
            case "AttackAll Card":
                return $"This card will attack all enemies for {attackAmount} damage.";
            case "TripleAttack Card":
                return $"This card will attack {attackAmount} damage.";
            case "BadAttack Card":
                return $"This card will attack {attackAmount} damage";
            case "LowAttack Card":
                return $"This card will attack {attackAmount} damage";
            default:
                return $"This card has no specific action.";
        }
    }
}
