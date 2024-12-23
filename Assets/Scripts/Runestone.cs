using UnityEngine;

public class Runestone : MonoBehaviour
{
    public int runestoneIndex; // The index of this runestone in the sequence
    private RunestonePuzzle puzzleManager;
    
    void Start()
    {
        puzzleManager = FindFirstObjectByType<RunestonePuzzle>(); // Find the puzzle manager in the scene
    }

    void OnMouseDown() // Or OnTriggerEnter for proximity-based interaction
    {
        puzzleManager.ActivateRunestone(runestoneIndex); // Notify the puzzle manager
    }
}
