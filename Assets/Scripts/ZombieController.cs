using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Start()
    {
        playerView.enabled = true;
        fightView.enabled = false;
        gameController = FindFirstObjectByType<GameControllerRoom3>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        roomCollider = GameObject.FindGameObjectWithTag("RoomArea").GetComponent<Collider>();
        animator = GetComponent<Animator>(); // Initialize the Animator
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

            targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
        }
        else if (isFollowingPlayer)
        {
            targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
        }
        else
        {
            isFollowingPlayer = false;

            targetPosition = new Vector3(patrolPoints[targetPointIndex].position.x, transform.position.y, patrolPoints[targetPointIndex].position.z);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                IncreaseTargetIndex();
                targetPosition = new Vector3(patrolPoints[targetPointIndex].position.x, transform.position.y, patrolPoints[targetPointIndex].position.z);
            }
        }

        if (!hasStartedFight)
        {
            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
            animator.SetBool("isWalking", true); // Play walking animation

            if (distanceToTarget > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                RotateTowards(targetPosition);
                
            }
            else
            {
                animator.SetBool("isWalking", false); // Stop walking animation
            }
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

        animator.SetBool("isWalking", false); // Stop walking animation for battle
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
}
