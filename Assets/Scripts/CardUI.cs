using UnityEngine;
using TMPro;

public class CardUI : MonoBehaviour
{
    public TextMeshProUGUI cardNameText; 

    // Method to set the card name
    public void SetCardName(string name)
    {
        cardNameText.text = name; // Set the name of the card in the UI
    }
}
