using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    public Transform[] patrolPoints;  // Array to hold the patrol points
    public int targetPointIndex = 0;  // Index of the current patrol point
    public float moveSpeed = 5f;  // Speed at which the enemy moves
    public bool isFollowingPlayer = false;  // Flag to indicate if the enemy should follow the player
    private Transform player;  // Reference to the player's transform
    private Collider roomCollider;  // The room area collider
    private bool isStopped = false;  // Flag to stop the enemy's movement
    private Animator animator;  // Reference to Animator
    public bool isEnemyDefeated = false; // Flag to indicate if the enemy is defeated
    private GameController gameController;
    public bool dropHealthPotion;  // Flag to indicate if the enemy should drop a Health potion
    private NavMeshAgent navMeshAgent;  // Reference to the NavMeshAgent

    void Start()
    {
        gameController = FindFirstObjectByType<GameController>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;  // Get the player
        roomCollider = GameObject.FindGameObjectWithTag("RoomArea").GetComponent<Collider>();  // Get the room area collider
        navMeshAgent = GetComponent<NavMeshAgent>();  // Get the NavMeshAgent component

        // Set up NavMeshAgent
        navMeshAgent.speed = moveSpeed; 
        navMeshAgent.updateRotation = false;
    }

    void Update()
    {
        if (isEnemyDefeated)
        {
            // If the enemy is defeated, make it disappear
            Disappear();
            if(dropHealthPotion) 
            {
                // Drop a health potion
                PlayerData.instance.AddItem("Health Potion", Resources.Load<Sprite>("HealthPotion"), "A Health Potion. Restores 20 health.");
            }

            if(SceneManager.GetActiveScene().name == "Room1") 
            {
                // Update the objective text
                PlayerData.instance.setObjective("Find the chest to get the key.");
            }


            return;  // Exit the update method to stop further behavior
        }

        animator.SetBool("isWalking", true); // Start walking animation

        // Check if the enemy is stopped
        if (isStopped)
        {
            animator.SetBool("isWalking", false);
            navMeshAgent.isStopped = true;  // Stop the NavMeshAgent's movement
            return;  // Exit the update method if the enemy is stopped
        }
        else
        {
            navMeshAgent.isStopped = false;  // Ensure the NavMeshAgent is active
        }

        Vector3 targetPosition;

        if (isFollowingPlayer || IsPlayerInsideRoom())  // Continue following if already chasing or inside room
        {
            isFollowingPlayer = true;  // Always chase after detecting the player

            // Target is the player's position (constrained to X and Z axes)
            targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
            navMeshAgent.SetDestination(targetPosition);  // Set the NavMeshAgent's destination
        }
        else
        {
            // Target is the current patrol point (constrained to X and Z axes)
            targetPosition = new Vector3(patrolPoints[targetPointIndex].position.x, transform.position.y, patrolPoints[targetPointIndex].position.z);
            navMeshAgent.SetDestination(targetPosition);  // Set the NavMeshAgent's destination

            // Check if the enemy reached the patrol point
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                IncreaseTargetIndex();
            }
        }

        // Rotate towards the target using the NavMeshAgent's velocity
        RotateTowards(navMeshAgent.velocity);
    }

    void RotateTowards(Vector3 velocity)
    {
        // If there's velocity, rotate the enemy to face the direction of movement
        if (velocity.magnitude > 0.1f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(velocity);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * moveSpeed);
        }
    }

    // When the player and enemy collide, start the fight
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isEnemyDefeated)
        {
            isFollowingPlayer = true;  // Ensure the enemy is chasing
            gameController.StartFight();  // Trigger the fight
        }
    }

    // Checks if the player is inside the room area
    bool IsPlayerInsideRoom()
    {
        if (roomCollider == null || player == null)
        {
            return false;
        }

        // Check if the player's position is inside the room collider's bounds
        return roomCollider.bounds.Contains(player.position);
    }

    void IncreaseTargetIndex()
    {
        targetPointIndex++;
        if (targetPointIndex >= patrolPoints.Length)
        {
            targetPointIndex = 0;
        }
    }

    // Method to stop the enemy's movement
    public void StopEnemy()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", false);
        isStopped = true;
    }

    // Method to resume the enemy's movement
    public void ResumeEnemy()
    {
        isStopped = false;
    }

    // Method to handle the disappearance of the enemy when defeated
    void Disappear()
    {
        Destroy(gameObject);  // Destroy the enemy GameObject 

    }
}
