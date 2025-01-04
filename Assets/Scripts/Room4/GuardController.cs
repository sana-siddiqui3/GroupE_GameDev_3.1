using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private GameControllerGuard gameController; // Reference to the game controller
    public bool hasStartedFight = false; // Whether the fight has started
    public Camera playerView; // Camera for the player's view
    public Camera fightView; // Camera for the fight view
    public Transform enemyFightPosition; // The position for the guard during the fight
    public Transform playerFightPosition; // The position for the player during the fight
    private GuardController[] allGuards; // All guards in the scene for separation
    private Animator animator; // Animator component

    void Start()
    {
        playerView.enabled = true;
        fightView.enabled = false;
        gameController = FindFirstObjectByType<GameControllerGuard>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        roomCollider = GameObject.FindGameObjectWithTag("RoomArea").GetComponent<Collider>();

        // Get all guards in the scene for separation behavior
        allGuards = FindObjectsByType<GuardController>(FindObjectsSortMode.None);

        // Get Animator component
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isEnemyDefeated)
        {
            gameObject.SetActive(false);
            return;
        }

        bool playerInsideRoom = IsPlayerInsideRoom();

        Vector3 targetPosition;
        if (playerInsideRoom && !hasStartedFight)
        {
            if (!isFollowingPlayer)
            {
                isFollowingPlayer = true;
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

            // Patrol between points
            targetPosition = patrolPoints[targetPointIndex].position;

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                IncreaseTargetIndex();
                targetPosition = patrolPoints[targetPointIndex].position;
            }
        }

        if (!hasStartedFight)
        {
            Vector3 separationOffset = CalculateSeparationOffset();
            Vector3 finalTarget = targetPosition + separationOffset;

            bool isWalking = Vector3.Distance(transform.position, finalTarget) > 0.1f;
            UpdateAnimationState(isWalking);

            transform.position = Vector3.MoveTowards(transform.position, finalTarget, moveSpeed * Time.deltaTime);
            RotateTowards(targetPosition);
        }
        else
        {
            UpdateAnimationState(false); // Idle during fights
        }
    }

    void StartBattle()
    {
        if (hasStartedFight) return;

        hasStartedFight = true;

        transform.position = enemyFightPosition.position;
        transform.rotation = enemyFightPosition.rotation;

        player.transform.position = playerFightPosition.position;
        player.transform.rotation = playerFightPosition.rotation;

        UpdateAnimationState(false); // Set to idle during fight
        gameController.StartFight();
    }

    void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0;

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
