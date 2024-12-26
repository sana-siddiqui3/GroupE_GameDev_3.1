using UnityEngine;
using TMPro;
using UnityEngine.InputSystem; // Add this line for the new input system

public class Card : MonoBehaviour
{
    public string cardName;

    private bool playerInRange = false;

    public TextMeshProUGUI pickupMessageText;

    void Update()
    {
        // Use the new Input System to check for "E" key press
        if (playerInRange && Keyboard.current.eKey.wasPressedThisFrame)
        {
            PickUp();
        }
    }

    public void PickUp()
    {
        PlayerData.instance.AddCard(cardName);
        pickupMessageText.text = ""; // Clears the pickup message
        Destroy(gameObject); // Removes the card from the scene
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            pickupMessageText.text = "Press 'E' to pick up " + cardName;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            pickupMessageText.text = "";
        }
    }
}