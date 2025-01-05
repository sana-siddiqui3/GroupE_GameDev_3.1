using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentObject : MonoBehaviour
{
    private static PersistentObject instance;

    private void Awake()
    {
        // Ensure only one instance of the object exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Make this object persistent across scenes
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicate objects when loading new scenes
        }
    }
}
