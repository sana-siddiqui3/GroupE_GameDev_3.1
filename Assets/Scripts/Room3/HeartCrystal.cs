using UnityEngine;

public class HeartCrystal : MonoBehaviour
{
    public float interactionDistance = 6f; // Distance at which the player can interact with the crystal
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Checks if the player is within the interaction distance of the heart crystal
    public bool IsPlayerNear()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        return distance <= interactionDistance;
    }
}
