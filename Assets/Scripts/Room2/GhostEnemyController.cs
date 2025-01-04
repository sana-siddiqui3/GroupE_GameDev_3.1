using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEnemyController : MonoBehaviour
{
    public Transform[] patrolPoints;
    public int targetPointIndex = 0;
    public float moveSpeed = 5f;
    public float separationDistance = 1.5f; // Minimum distance between ghosts
    public float separationStrength = 2f; // Strength of the separation force
    public bool isFollowingPlayer = false;
    private Transform player;
    private Collider roomCollider;
    public bool isEnemyDefeated = false;
    private GameControllerRoom2 gameController;
    public bool hasStartedFight = false;
    public Camera playerView;
    public Camera fightView;
    public Transform enemyFightPosition;
    public Transform playerFightPosition;
    private GhostEnemyController[] allGhosts;

    void Start()
    {
        playerView.enabled = true;
        fightView.enabled = false;
        gameController = FindFirstObjectByType<GameControllerRoom2>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        roomCollider = GameObject.FindGameObjectWithTag("RoomArea").GetComponent<Collider>();

        // Get all ghosts in the scene for separation behavior
        allGhosts = FindObjectsByType<GhostEnemyController>(FindObjectsSortMode.None);
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

            transform.position = Vector3.MoveTowards(transform.position, finalTarget, moveSpeed * Time.deltaTime);
            RotateTowards(targetPosition);
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

        foreach (GhostEnemyController ghost in allGhosts)
        {
            if (ghost != this && !ghost.isEnemyDefeated)
            {
                float distance = Vector3.Distance(transform.position, ghost.transform.position);
                if (distance < separationDistance)
                {
                    // Push away from nearby ghosts
                    Vector3 awayFromGhost = (transform.position - ghost.transform.position).normalized;
                    separation += awayFromGhost * (separationDistance - distance) * separationStrength;
                }
            }
        }

        return separation;
    }
}
