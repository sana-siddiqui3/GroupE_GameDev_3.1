using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEnemyController : MonoBehaviour
{
    public Transform[] patrolPoints; // Array to hold the patrol points
    public int targetPointIndex = 0; // Index of the current target point

    public float moveSpeed = 5f; // Speed at which the ghost moves
    public bool isFollowingPlayer = false; // Flag to indicate if the ghost should follow the player
    private Transform player; // Reference to the player's transform
    private Collider roomCollider; // The room area collider

    private float chaseDistance = 0.1f; // Distance at which the ghost catches the player
    public bool isEnemyDefeated = false; // Flag to indicate if the ghost is defeated
    private GameControllerRoom2 gameController;

    private bool hasStartedFight = false; // Flag to ensure the fight only starts once
    public Camera playerView;
    public Camera fightView;

    public Transform enemyFightPosition; // The position where the ghost should go for the fight
    public Transform playerFightPosition; // The position where the player should go for the fight

    void Start()
    {
        playerView.enabled = true;
        fightView.enabled = false;
        gameController = FindFirstObjectByType<GameControllerRoom2>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // Get the player
        roomCollider = GameObject.FindGameObjectWithTag("RoomArea").GetComponent<Collider>(); // Get the room area collider
    }

    void Update()
    {
        // Check if the ghost is defeated
        if (isEnemyDefeated) return;

        bool playerInsideRoom = IsPlayerInsideRoom();

        // Determine the target position and handle movement
        Vector3 targetPosition;
        if (playerInsideRoom && !hasStartedFight)
        {
            if (!isFollowingPlayer)
            {
                isFollowingPlayer = true; // Start following the player if inside the room
            }

            // Target is the player's position (constrained to X and Z axes)
            targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
        }
        else
        {
            isFollowingPlayer = false; // Stop following the player

            // Target is the current patrol point (constrained to X and Z axes)
            targetPosition = new Vector3(patrolPoints[targetPointIndex].position.x, transform.position.y, patrolPoints[targetPointIndex].position.z);

            // Check if the ghost reached the patrol point
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                IncreaseTargetIndex();
                targetPosition = new Vector3(patrolPoints[targetPointIndex].position.x, transform.position.y, patrolPoints[targetPointIndex].position.z);
            }
        }

        // Move towards the target position if the fight hasn't started
        if (!hasStartedFight)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Rotate to face the target position
            RotateTowards(targetPosition);
        }

        // Check distance to player
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // If the distance is within chase range and the fight has not started
        if (distanceToPlayer <= chaseDistance && !isEnemyDefeated && !hasStartedFight)
        {
            Debug.Log("Ghost caught the player!");
            StartBattle();
        }
    }

    void StartBattle()
    {
        hasStartedFight = true; // Prevent multiple triggers

        // Move the ghost and player to their fight positions
        transform.position = enemyFightPosition.position;
        transform.rotation = enemyFightPosition.rotation;

        player.transform.position = playerFightPosition.position;
        player.transform.rotation = playerFightPosition.rotation;

        // Trigger the fight in the GameController
        gameController.StartFight();
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
            // Create a rotation that points towards the target
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            // Preserve the X rotation from the current rotation while applying the Y rotation
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, lookRotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
    }


    // When the player enters the room area, start following the player
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isFollowingPlayer = true; // Start chasing the player
        }
    }

    // When the player exits the room area, stop following the player
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isFollowingPlayer = false; // Stop chasing the player
        }
    }

    // Checks if the player is inside the room area
    bool IsPlayerInsideRoom()
    {
        if (roomCollider == null || player == null) return false;

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
}
