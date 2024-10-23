using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace Assets.Scripts
{
    public class RoomController : MonoBehaviour
    {
        [SerializeField] private GameObject enterRoomUI = null;
        [SerializeField] private GameObject Door = null;

        void Start()
        {
            enterRoomUI.SetActive(false); // Hide the enter room UI
        }

        public void BtnYes()
        {
            StartCoroutine(enterRoom()); // Load the room when 'Yes' pressed
        }

        public void BtnNo()
        {
            enterRoomUI.SetActive(false); // Hide the enter room UI when 'No' pressed
        }

        IEnumerator enterRoom()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(Door.tag); // Load the new scene asynchronously

            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
    }
}
