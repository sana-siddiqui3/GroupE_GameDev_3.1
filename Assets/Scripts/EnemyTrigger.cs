using UnityEngine;
using UnityEngine.UI;

public class EnemyTrigger : MonoBehaviour
{
    public GameObject fightPromptUI;  // The fight prompt message
    public GameObject fightUI;        // The UI elements for the actual fight (health bars, attack buttons, etc.)
    private bool playerInRange = false;

    void Start()
    {
        // Hide both the fight prompt and the fight UI at the start
        fightPromptUI.SetActive(false);
        fightUI.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        // When the player enters the trigger, show the fight prompt
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            fightPromptUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Hide the fight prompt if the player leaves the trigger area
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            fightPromptUI.SetActive(false);
        }
    }

    void Update()
    {
        // If the player is in range and presses the "Enter" key, start the fight
        if (playerInRange && Input.GetKeyDown(KeyCode.Return))
        {
            StartFight();
        }
    }

    void StartFight()
    {
        // Hide the fight prompt
        fightPromptUI.SetActive(false);

        // Show the fight UI
        fightUI.SetActive(true);

        // Trigger the fight logic (which you already have implemented)
        Debug.Log("Fight Started!");
        // Example: FightManager.StartFight();
    }
}
