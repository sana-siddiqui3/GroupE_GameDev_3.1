using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))  // Press 'E' to pick up
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 3f))  // Adjust range as needed
            {
                Card card = hit.collider.GetComponent<Card>();
                if (card != null)
                {
                    card.PickUp();
                }
            }
        }
    }
}
