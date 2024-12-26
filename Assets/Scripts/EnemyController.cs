using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform[] patrolPoints;  // Array to hold the patrol points
    public int targetPointIndex = 0;  // Index of the current target point

    public float moveSpeed = 5f;  // Speed at which the enemy moves
    public bool isFollowingPlayer = false;  // Flag to indicate if the enemy should follow the player
    private Transform player;  // Reference to the player's transform
    private Collider roomCollider;  // The room area collider

    private bool isStopped = false;  // Flag to stop the enemy's movement
    private Animator animator;  // Reference to Animator

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;  // Get the player
        roomCollider = GameObject.FindGameObjectWithTag("RoomArea").GetComponent<Collider>();  // Get the room area collider
    }

    void Update()
    {
        animator.SetBool("isWalking", true); // Start walking animation

        // Check if the enemy is stopped
        if (isStopped)
        {
            animator.SetBool("isWalking", false);
            return;  // Exit the update method if the enemy is stopped
        }

        bool playerInsideRoom = IsPlayerInsideRoom();

        // Determine the target position and handle movement
        Vector3 targetPosition;
        if (playerInsideRoom)
        {
            if (!isFollowingPlayer)
            {
                isFollowingPlayer = true;  // Start following the player if inside room
            }

            // Target is the player's position (constrained to X and Z axes)
            targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
        }
        else
        {
            isFollowingPlayer = false;  // Stop following the player

            // Target is the current patrol point (constrained to X and Z axes)
            targetPosition = new Vector3(patrolPoints[targetPointIndex].position.x, transform.position.y, patrolPoints[targetPointIndex].position.z);

            // Check if the enemy reached the patrol point
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                IncreaseTargetIndex();
                targetPosition = new Vector3(patrolPoints[targetPointIndex].position.x, transform.position.y, patrolPoints[targetPointIndex].position.z);
            }
        }

        // Move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Rotate to face the target position
        RotateTowards(targetPosition);
    }

    void RotateTowards(Vector3 targetPosition)
    {
        // Calculate the direction to the target
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Ignore changes in the Y-axis to prevent tilting
        direction.y = 0;

        // If there is a significant direction change, apply rotation
        if (direction.magnitude > 0.1f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * moveSpeed);
        }
    }

    // When the player enters the room area, start following the player
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isFollowingPlayer = true;  // Start chasing the player
        }
    }

    // When the player exits the room area, stop following the player
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isFollowingPlayer = false;  // Stop chasing the player
        }
    }

    // Checks if the player is inside the room area
    bool IsPlayerInsideRoom()
    {
        if (roomCollider == null)
        {
            return false;
        }

        if (player == null)
        {
            return false;
        }

        // Check if the player's position is inside the room collider's bounds
        bool isInside = roomCollider.bounds.Contains(player.position);

        return isInside;
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

    void OnDrawGizmos()
    {
        if (roomCollider != null)
        {
            // Set Gizmo color
            Gizmos.color = Color.green;

            // Draw a wireframe box around the collider bounds
            Gizmos.DrawWireCube(roomCollider.bounds.center, roomCollider.bounds.size);
        }
    }
}
