using UnityEngine;
using TMPro;


public class CardUI : MonoBehaviour

{

    public TextMeshProUGUI cardNameText;



    public void SetCardName(string cardName)

    {

        cardNameText.text = cardName;

    }



    public string GetCardName()

    {

        return cardNameText.text;

    }

}

