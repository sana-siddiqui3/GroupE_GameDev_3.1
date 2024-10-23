using UnityEngine;
using System.Collections;

public class CompanionTrigger : MonoBehaviour
{
    public GameObject dialoguePromptUI;  
    public GameObject dialogueUI;       
    public GameObject player;

    private bool playerInRange = false; 

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
            dialoguePromptUI.SetActive(true); // Show the prompt
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

    // Check if the player presses the Enter while in range
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
        dialoguePromptUI.SetActive(false);  
        dialogueUI.SetActive(true);         
        Debug.Log("Dialogue triggered with the companion.");

        StartCoroutine(HideDialogueAfterDelay());
    }

    // Coroutine to hide the dialogue UI after 3 seconds
    IEnumerator HideDialogueAfterDelay()
    {
        yield return new WaitForSeconds(3);  
        dialogueUI.SetActive(false);        
        Debug.Log("Dialogue hidden.");
    }
}
