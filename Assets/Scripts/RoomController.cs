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
            enterRoomUI.SetActive(false);
        }

        public void BtnYes()
        {
            StartCoroutine(enterRoom());
        }

        public void BtnNo()
        {
            enterRoomUI.SetActive(false);
        }

        IEnumerator enterRoom()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(Door.tag);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
    }
}
