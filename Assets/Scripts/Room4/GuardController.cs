using UnityEngine;
using UnityEngine.AI;  // For NavMeshAgent

public class GuardController : MonoBehaviour
{
    public Transform[] patrolPoints; // Patrol points for the guards
    public int targetPointIndex = 0; // The current patrol point index
    public float moveSpeed = 5f; // Movement speed of the guards
    public float separationDistance = 1.5f; // Minimum distance between guards
    public float separationStrength = 2f; // Strength of the separation force
    public bool isFollowingPlayer = false; // Whether the guard is following the player
    private Transform player; // Reference to the player
    private Collider roomCollider; // Room collider to check if player is inside
    public bool isEnemyDefeated = false; // Whether the guard is defeated
    private GameControllerRoom4 gameController; // Reference to the game controller
    public bool hasStartedFight = false; // Whether the fight has started
    public Camera playerView; // Camera for the player's view
    public Camera fightView; // Camera for the fight view
    private GuardController[] allGuards; // All guards in the scene for separation
    private Animator animator; // Animator component
    private NavMeshAgent navMeshAgent; // NavMeshAgent component for pathfinding

    void Start()
    {
        playerView.enabled = true;
        fightView.enabled = false;
        gameController = FindFirstObjectByType<GameControllerRoom4>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        roomCollider = GameObject.FindGameObjectWithTag("RoomArea2").GetComponent<Collider>();
        allGuards = FindObjectsByType<GuardController>(FindObjectsSortMode.None);
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>(); // Get the NavMeshAgent component
        navMeshAgent.speed = moveSpeed; // Set the guard's move speed
        navMeshAgent.autoBraking = false; // Optional: Prevent the agent from braking when reaching the target
        navMeshAgent.enabled = false; // Start with the NavMeshAgent disabled for patrolling
    }

    void Update()
    {
        if (isEnemyDefeated)
        {
            gameObject.SetActive(false);
            PlayerData.instance.setObjective("Defeat the boss to win the game!");
            CardTooltip.instance.HideTooltip(); // Hide the tooltip
            return;
        }

        bool playerInsideRoom = IsPlayerInsideRoom();

        Vector3 targetPosition;
        if (playerInsideRoom && !hasStartedFight)
        {
            if (!isFollowingPlayer)
            {
                isFollowingPlayer = true;
                navMeshAgent.enabled = true; // Enable the NavMeshAgent when chasing the player
            }

            // Follow the player's exact position
            targetPosition = player.position;
        }
        else if (isFollowingPlayer)
        {
            // Continue following the player
            targetPosition = player.position;
        }
        else
        {
            isFollowingPlayer = false;
            navMeshAgent.enabled = false; // Disable the NavMeshAgent when patrolling

            // Patrol between points manually
            targetPosition = patrolPoints[targetPointIndex].position;

            // If the guard has reached the patrol point, update the target point
            if (Vector3.Distance(transform.position, targetPosition) < 0.5f)
            {
                IncreaseTargetIndex();
                targetPosition = patrolPoints[targetPointIndex].position;
            }

            // Manually move the guard when NavMeshAgent is disabled
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }

        if (!hasStartedFight)
        {
            Vector3 separationOffset = CalculateSeparationOffset();
            Vector3 finalTarget = targetPosition + separationOffset;

            bool isWalking = Vector3.Distance(transform.position, finalTarget) > 0.1f;
            UpdateAnimationState(isWalking);

            if (navMeshAgent.enabled)
            {
                // Set the NavMeshAgent's destination when it's enabled
                navMeshAgent.SetDestination(finalTarget);
            }

            // Turn the guard towards the target position when moving
            RotateTowards(finalTarget);
        }
        else
        {
            UpdateAnimationState(false); // Idle during fights
            // Stop the movement when the battle starts
            if (navMeshAgent.enabled) navMeshAgent.isStopped = true;
        }
    }

    void StartBattle()
    {
        if (hasStartedFight) return;

        hasStartedFight = true;
        UpdateAnimationState(false); // Set to idle during fight
        gameController.StartFight();
    }

    void RotateTowards(Vector3 targetPosition)
    {
        // Rotate the guard to face the target position
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0; // Keep the rotation on the y-axis only

        if (direction.magnitude > 0.1f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, lookRotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isEnemyDefeated && !hasStartedFight)
        {
            StartBattle();
        }
    }

    bool IsPlayerInsideRoom()
    {
        if (roomCollider == null || player == null) return false;

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

    Vector3 CalculateSeparationOffset()
    {
        Vector3 separation = Vector3.zero;

        foreach (GuardController guard in allGuards)
        {
            if (guard != this && !guard.isEnemyDefeated)
            {
                float distance = Vector3.Distance(transform.position, guard.transform.position);
                if (distance < separationDistance)
                {
                    // Push away from nearby guards
                    Vector3 awayFromGuard = (transform.position - guard.transform.position).normalized;
                    separation += awayFromGuard * (separationDistance - distance) * separationStrength;
                }
            }
        }

        return separation;
    }

    void UpdateAnimationState(bool isWalking)
    {
        if (animator != null)
        {
            animator.SetBool("isWalking", isWalking);
        }
    }
}
