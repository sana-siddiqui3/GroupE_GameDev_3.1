using UnityEngine;

public class FallTrigger : MonoBehaviour
{
    public PathGenerator bridgeManager;  // Reference to the GlassBridgeManager

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Notify the GlassBridgeManager that the player has fallen
            bridgeManager.PlayerFell();
        }
    }
}
