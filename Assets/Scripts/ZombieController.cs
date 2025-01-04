using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 

public class ZombieController : MonoBehaviour
{
    public Transform[] patrolPoints;
    public int targetPointIndex = 0;
    public float moveSpeed = 5f;
    public bool isFollowingPlayer = false;
    private Transform player;
    private Collider roomCollider;
    public bool isEnemyDefeated = false;
    private GameControllerRoom3 gameController;
    public bool hasStartedFight = false;
    public Camera playerView;
    public Camera fightView;
    public Transform enemyFightPosition;
    public Transform playerFightPosition;
    private Animator animator; // Reference to the Animator
    private NavMeshAgent navAgent; // NavMeshAgent component reference

    void Start()
    {
        playerView.enabled = true;
        fightView.enabled = false;
        gameController = FindFirstObjectByType<GameControllerRoom3>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        roomCollider = GameObject.FindGameObjectWithTag("RoomArea").GetComponent<Collider>();
        animator = GetComponent<Animator>();

        navAgent = GetComponent<NavMeshAgent>(); 
        navAgent.speed = moveSpeed; 
    }

    void Update()
{
    if (isEnemyDefeated)
    {
        gameObject.SetActive(false);
        return;
    }

    bool playerInsideRoom = IsPlayerInsideRoom();

    if (hasStartedFight)
    {
        // Stop following the player when the fight starts
        isFollowingPlayer = false;

        // Ensure the zombie goes to idle animation and stops walking
        animator.SetBool("isWalking", false); 

        // Position the zombies in their designated positions for the fight
        if (!isEnemyDefeated) 
        {
            navAgent.enabled = false; // Disable NavMeshAgent for fight
            transform.position = enemyFightPosition.position;
            transform.rotation = enemyFightPosition.rotation;
        }
    }
    else
    {
        if (playerInsideRoom && !hasStartedFight)
        {
            if (!isFollowingPlayer)
            {
                isFollowingPlayer = true;
            }

            navAgent.SetDestination(new Vector3(player.position.x, transform.position.y, player.position.z));
        }
        else if (isFollowingPlayer)
        {
            navAgent.SetDestination(new Vector3(player.position.x, transform.position.y, player.position.z));
        }
        else
        {
            isFollowingPlayer = false;

            Vector3 targetPosition = new Vector3(patrolPoints[targetPointIndex].position.x, transform.position.y, patrolPoints[targetPointIndex].position.z);
            navAgent.SetDestination(targetPosition);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                IncreaseTargetIndex();
            }
        }
    }

    // If not in battle, play walking animation when moving
    if (!hasStartedFight)
    {
        animator.SetBool("isWalking", navAgent.velocity.sqrMagnitude > 0.1f);
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

    // Stop walking animation for battle
    animator.SetBool("isWalking", false);
    gameController.StartFight();
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
}
