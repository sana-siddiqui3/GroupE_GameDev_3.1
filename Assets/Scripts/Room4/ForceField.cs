using UnityEngine;

public class InvisibleForceField : MonoBehaviour
{
    private Collider forceFieldCollider;
    private GameControllerRoom4 gameController; // Reference to the GameControllerFinalBoss

    void Start()
    {
        forceFieldCollider = GetComponent<Collider>(); // Get the collider on the force field
        gameController = FindFirstObjectByType<GameControllerRoom4>(); // Reference to the GameControllerFinalBoss
    }

    void Update()
    {
        // Check if the guards are defeated
        if (gameController.IsVictoryAchieved())
        {
            RemoveForceField();
        }
        else
        {
            EnableForceField();
        }
    }

    void EnableForceField()
    {
        // Ensure the force field is active and blocking the player
        if (forceFieldCollider != null)
        {
            forceFieldCollider.enabled = true; // Blocks the player
        }
    }

    void RemoveForceField()
    {
        // Remove the force field to let the player pass through
        if (forceFieldCollider != null)
        {
            forceFieldCollider.enabled = false; // Makes the force field disappear
        }
    }
}
