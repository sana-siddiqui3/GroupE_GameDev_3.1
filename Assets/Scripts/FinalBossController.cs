using UnityEngine;

public class FinalBossController : MonoBehaviour
{
    public Transform[] patrolPoints; // Array to hold the patrol points
    public int targetPointIndex = 0; // Index of the current patrol point
    public float moveSpeed = 5f; // Speed at which the enemy moves
    public bool isFollowingPlayer = false; // Flag to indicate if the enemy should follow the player
    private Transform player; // Reference to the player's transform
    private Collider roomCollider; // The room area collider
    private bool isStopped = false; // Flag to stop the enemy's movement
    private Animator animator; // Reference to Animator
    public bool isEnemyDefeated = false; // Flag to indicate if the enemy is defeated
    private GameController gameController;
    private bool isFightActive = false; // Flag to track if a fight is active

    void Start()
    {
        gameController = FindFirstObjectByType<GameController>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // Get the player
        roomCollider = GameObject.FindGameObjectWithTag("RoomArea").GetComponent<Collider>(); // Get the room area collider
    }

    void Update()
    {
        if (isEnemyDefeated)
        {
            Disappear(); // Handle defeat
            return; // Exit the update method
        }

        animator.SetBool("isWalking", true); // Start walking animation

        if (isStopped || isFightActive)
        {
            animator.SetBool("isWalking", false);
            return; // Exit if the enemy is stopped or fight is active
        }

        Vector3 targetPosition;

        if (isFollowingPlayer || IsPlayerInsideRoom()) // Follow the player if detected
        {
            isFollowingPlayer = true;

            // Target the player's position
            targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
        }
        else
        {
            // Patrol between points
            targetPosition = new Vector3(patrolPoints[targetPointIndex].position.x, transform.position.y, patrolPoints[targetPointIndex].position.z);

            // Check if the enemy reached the patrol point
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                IncreaseTargetIndex();
            }
        }

        // Move toward the target position
        MoveTowards(targetPosition);
    }

    void MoveTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        if (direction.magnitude > 0.1f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * moveSpeed);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isEnemyDefeated)
        {
            isFollowingPlayer = false; // Stop following the player
            isFightActive = true; // Set fight active
            StopEnemy(); // Ensure the enemy stops moving
            gameController.StartFight(); // Trigger the fight
        }
    }

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

    public void StopEnemy()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", false);
        isStopped = true;
    }

    public void ResumeEnemy()
    {
        isStopped = false;
    }

    void Disappear()
    {
        Destroy(gameObject);
    }

    public void EndFight()
    {
        isFightActive = false; // Allow movement after the fight ends
    }
}
