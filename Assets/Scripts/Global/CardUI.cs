using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class CardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI cardNameText;
    public GameObject hoverInfoPanel; // A UI element that shows the card details
    public TextMeshProUGUI hoverInfoText; // Text component for the hover info

    private string cardName;
    private int energyCost;
    private int attackAmount;

    public void SetCardDetails(string cardName, int energyCost, int attackAmount)
    {
        this.cardName = cardName;
        this.energyCost = energyCost;
        this.attackAmount = attackAmount;
        cardNameText.text = cardName;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverInfoPanel != null)
        {
            hoverInfoPanel.SetActive(true);
            hoverInfoText.text = $"Energy: {energyCost}\nAttack: {attackAmount}";
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (hoverInfoPanel != null)
        {
            hoverInfoPanel.SetActive(false);
        }
    }
}
