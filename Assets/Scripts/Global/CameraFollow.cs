using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Reference to the player
    public Vector3 offset; // Offset from the player
    public float smoothSpeed = 0.125f; // Smoothness of the camera movement
    public float collisionBuffer = 0.5f; // Distance to maintain from obstacles

    private Vector3 currentVelocity; // Current velocity for smooth damping

    void LateUpdate()
    {
        // Calculate the desired position of the camera
        Vector3 desiredPosition = player.position + offset;

        // Raycast to check if the camera would collide with any object
        RaycastHit hit;
        if (Physics.Raycast(player.position, offset.normalized, out hit, offset.magnitude))
        {
            // If there's a collision, move the camera closer to the player
            desiredPosition = hit.point - offset.normalized * collisionBuffer;
        }

        // Smoothly transition to the new position
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothSpeed);
        
        // Always look at the player
        transform.LookAt(player);
    }
}
