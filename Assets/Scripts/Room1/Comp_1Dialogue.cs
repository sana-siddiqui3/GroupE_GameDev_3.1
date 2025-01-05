using UnityEngine;
using UnityEngine.InputSystem; // Add this line for the new input system
using System.Collections;

public class CompanionTrigger : MonoBehaviour
{
    public GameObject dialoguePromptUI;
    public GameObject dialogueUI;
    public GameObject player;

    private bool playerInRange = false;

    // Reference to Input Actions
    private PlayerInput controls;
    private InputAction interactAction;

    void Awake()
    {
        // Get the input actions from the PlayerControls asset
        controls = new PlayerInput();
        interactAction = controls.Player.Interact;  // Interact action
    }

    void OnEnable()
    {
        controls.Enable(); // Enable the input actions
    }

    void OnDisable()
    {
        controls.Disable(); // Disable the input actions
    }

    void Start()
    {
        dialoguePromptUI.SetActive(false);
        dialogueUI.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            dialoguePromptUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            dialoguePromptUI.SetActive(false);
        }
    }

    void Update()
    {
        // Check for interaction input when the player is in range
        if (playerInRange && interactAction.triggered)
        {
            TriggerDialogue();
        }
    }

    void TriggerDialogue()
    {
        dialoguePromptUI.SetActive(false);
        dialogueUI.SetActive(true);
        Debug.Log("Dialogue triggered with the companion.");
        StartCoroutine(HideDialogueAfterDelay());
    }

    IEnumerator HideDialogueAfterDelay()
    {
        yield return new WaitForSeconds(10);
        dialogueUI.SetActive(false);
        Debug.Log("Dialogue hidden.");
    }
}
