using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image cardImage; // Reference to the Image component for the card sprite
    public string cardName; // The name of the card

    public void SetCardSprite(Sprite cardSprite)
    {
        if (cardImage != null)
        {
            cardImage.sprite = cardSprite;
            cardImage.enabled = true; // Ensure the image is visible
        }
    }

    public void SetCardName(string name)
    {
        cardName = name;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer entered card: " + cardName);
        // Show the tooltip when hovering over the card
        if (CardTooltip.instance != null)
        {
            CardTooltip.instance.ShowTooltip(cardName, Input.mousePosition);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Hide the tooltip when the cursor leaves the card
        if (CardTooltip.instance != null)
        {
            CardTooltip.instance.HideTooltip();
        }
    }

        public TextMeshProUGUI cardNameText;

    public void SetCardNameText(string cardName)
    {
        cardNameText.text = cardName;
    }

    public string GetCardName()
    {
        return cardNameText.text;
    }
}


