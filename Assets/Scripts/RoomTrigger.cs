using UnityEngine;
using UnityEngine.UI;

public class RoomTrigger : MonoBehaviour
{
    public GameObject roomEnterPromptUI;
    public GameObject player;
    public GameObject RoomChangeUI;

    private bool playerInRange = false;

    void Start()
    {
        roomEnterPromptUI.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            roomEnterPromptUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            roomEnterPromptUI.SetActive(false);
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            enterRoom();
        }
    }

    void enterRoom()
    {
        Debug.Log("entered new room");

        roomEnterPromptUI.SetActive(false);

        RoomChangeUI.SetActive(true);
    }
}
