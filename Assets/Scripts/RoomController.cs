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
        
        // Add a field to specify which scene to load
        [SerializeField] private string sceneToLoad = "";

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
            // Load the scene directly by name (no need for tag)
            if (!string.IsNullOrEmpty(sceneToLoad))
            {
                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);

                while (!asyncLoad.isDone)
                {
                    yield return null;
                }
            }
            else
            {
                Debug.LogError("Scene name is not assigned!");
            }
        }
    }
}
