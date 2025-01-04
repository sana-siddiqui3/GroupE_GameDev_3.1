using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentCanvas : MonoBehaviour
{
    private static PersistentCanvas instance;

    private void Awake()
    {
        // Ensure only one instance of the Canvas exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Make this Canvas persistent across scenes
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicate Canvases when loading new scenes
        }
    }
}
