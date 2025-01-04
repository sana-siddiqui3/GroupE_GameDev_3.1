using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class Chest : MonoBehaviour
{
    private bool isOpen = false;
    private bool playerInRange = false;
    private bool empty = false;
    public TextMeshProUGUI chestMessageText;

    void Start()
    {
        chestMessageText.text = "";
    }

    void Update()
    {
        if (playerInRange && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (!isOpen)
            {
                isOpen = true;
                GivePurityPotion();
            }
        }
    }

    private void GivePurityPotion()
    {
        if (!empty)
        {
            PlayerData.instance.AddItem("Purity Potion", Resources.Load<Sprite>("PurityPotion"), "A potion to purify the corrupted heart crystal.");
            chestMessageText.text = "Added Purity Potion.";
            empty = true;
        }
        else
        {
            chestMessageText.text = "This chest is empty.";
        }
        isOpen = false;
    }

    private void OnTriggerEnter(Collider other)
{
    Debug.Log("Entered trigger with: " + other.gameObject.name);
    if (other.CompareTag("Player"))
    {
        playerInRange = true;
        chestMessageText.text = "Press 'E' to open chest";
    }
}

private void OnTriggerExit(Collider other)
{
    Debug.Log("Exited trigger with: " + other.gameObject.name);
    if (other.CompareTag("Player"))
    {
        playerInRange = false;
        chestMessageText.text = "";
    }
}

}
