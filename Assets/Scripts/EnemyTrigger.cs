using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    public GameObject fightPromptUI; // UI element that shows when the player can fight
    public GameObject fightUI;      // UI element for the fight screen
    public Camera playerView;       // Camera for the player's view
    public Camera fightView;        // Camera for the battle view
    public GameObject player;       // Reference to the player object
    public GameObject enemy;        // Reference to the enemy object
    public Transform playerFightPosition; // Predefined player position for the fight
    public Transform enemyFightPosition;  // Predefined enemy position for the fight
    private bool isChasingPlayer = false; // Flag to check if the enemy is chasing the player
    private float chaseDistance = 2f;    // Distance at which the enemy catches the player
    public bool isEnemyDefeated = false; // Flag to indicate if the enemy is defeated
    private GameController gameController; // Reference to the GameController script
    private EnemyController enemyController; // Reference to the EnemyController script

    void Start()
    {
        fightPromptUI.SetActive(false); // Hide fight prompt initially
        fightUI.SetActive(false);      // Hide the battle UI initially
        gameController = FindFirstObjectByType<GameController>(); // Get reference to GameController
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isChasingPlayer = true; // Enemy starts chasing the player
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isChasingPlayer = false; // Stop chasing the player if they leave the area
        }
    }

    void Update()
    {
        if (isChasingPlayer)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distanceToPlayer <= chaseDistance && !isEnemyDefeated)
            {
                StartFight(); // Start the fight when the enemy catches the player
            }
        }
    }

    void StartFight()
{
    // Hide the fight prompt and show the fight UI
    fightPromptUI.SetActive(false);
    fightUI.SetActive(true);

    // Reset the player's and enemy's positions to predefined fight positions
    player.transform.position = playerFightPosition.position;
    player.transform.rotation = playerFightPosition.rotation;

    enemy.transform.position = enemyFightPosition.position;
    enemy.transform.rotation = enemyFightPosition.rotation;

    // Switch cameras to the fight view
    playerView.enabled = false;
    fightView.enabled = true;

    // Stop enemy movement
    enemyController = enemy.GetComponent<EnemyController>(); // Get the EnemyController component
    if (enemyController != null)
    {
        enemyController.StopEnemy(); // Stop the enemy's movement
    }

    if (gameController != null)
    {
        gameController.FightUI.SetActive(true); // Activate the fight UI in the GameController
        gameController.InitializeDeck();
        gameController.DrawCards(5);
    }
}

}
