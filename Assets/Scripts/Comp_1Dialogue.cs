using UnityEngine;
using System.Collections;

public class CompanionTrigger : MonoBehaviour
{
    public GameObject dialoguePromptUI;  // UI to show when player is in range (e.g., "Press Enter to talk")
    public GameObject dialogueUI;        // The actual dialogue UI (where the conversation is shown)
    public GameObject player;

    private bool playerInRange = false;  // Check if player is in range to interact

    void Start()
    {
        // Ensure UIs are hidden initially
        dialoguePromptUI.SetActive(false);
        dialogueUI.SetActive(false);
    }

    // Detect when the player enters the interaction zone
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            dialoguePromptUI.SetActive(true);  // Show prompt to press Enter
        }
    }

    // Detect when the player leaves the interaction zone
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            dialoguePromptUI.SetActive(false);  // Hide the prompt
        }
    }

    // Check if the player presses the interaction key (Enter) while in range
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.Return))
        {
            TriggerDialogue();
        }
    }

    // Show the dialogue when the player interacts
    void TriggerDialogue()
    {
        dialoguePromptUI.SetActive(false);  // Hide the prompt
        dialogueUI.SetActive(true);         // Show the dialogue UI
        Debug.Log("Dialogue triggered with the companion.");

        // Start coroutine to hide the dialogue after 3 seconds
        StartCoroutine(HideDialogueAfterDelay());
    }

    // Coroutine to hide the dialogue UI after 3 seconds
    IEnumerator HideDialogueAfterDelay()
    {
        yield return new WaitForSeconds(3);  // Wait for 3 seconds
        dialogueUI.SetActive(false);         // Hide the dialogue UI
        Debug.Log("Dialogue hidden.");
    }
}
