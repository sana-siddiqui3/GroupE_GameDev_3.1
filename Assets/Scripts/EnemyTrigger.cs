using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTrigger : MonoBehaviour
{
    public GameObject fightPromptUI;  
    public GameObject fightUI; 
    public Camera playerView;
    public Camera fightView;
    public GameObject player;
    private Vector3 fightingPosition;
    private bool playerInRange = false;
    public bool isEnemyDefeated = false; 
    private GameController gameController; 

    void Awake()
    {
        fightingPosition = player.transform.position;
        Debug.Log(fightingPosition);
    }

    void Start()
    {
        // Hide the fight UI initially
        fightPromptUI.SetActive(false);
        fightUI.SetActive(false);  

        // Get the GameController script to manage the fight
        gameController = FindFirstObjectByType<GameController>();
    }

    // Triggered when the player enters the collider
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;  
            fightPromptUI.SetActive(true); 
        }
    }

    // Triggered when the player exits the collider
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false; 
            fightPromptUI.SetActive(false); 
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.Return) && !isEnemyDefeated)
        {
            StartFight();  // Start the fight if the player is in range and hasn't defeated the enemy
        }
    }

    // Initiates the fight sequence and transitions to the fight UI
    void StartFight()
    {
        fightPromptUI.SetActive(false); 
        fightUI.SetActive(true);  

        playerView.enabled = false;  
        fightView.enabled = true; 

        Debug.Log("Fight Started!");

        // Show the player's cards in the battle scene
        BattleCardDisplay battleCardDisplay = FindFirstObjectByType<BattleCardDisplay>();
        if (battleCardDisplay != null)
        {
            battleCardDisplay.UpdateCardDisplay();  
        }

        if (gameController != null)
        {
            gameController.FightUI.SetActive(true); 
        }
    }
}
