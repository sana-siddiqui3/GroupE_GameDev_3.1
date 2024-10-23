using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Pick up card when 'E' key is pressed
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 3f))
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
