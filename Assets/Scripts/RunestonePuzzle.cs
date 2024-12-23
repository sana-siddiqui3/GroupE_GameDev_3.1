using UnityEngine;

public class RunestonePuzzle : MonoBehaviour
{
    public GameObject[] runestones; // Array to hold the runestones
    private int currentOrder = 0; // Tracks the player's progress through the sequence

    public GameObject doorToUnlock; // The door or passage that unlocks upon completion

    // Method to activate a runestone
    public void ActivateRunestone(int index)
    {
        if (index == currentOrder) // Check if the correct runestone is activated
        {
            Debug.Log("Correct Runestone Activated!");
            currentOrder++;

            if (currentOrder == 2 ) // If all runestones are activated
            {
                Debug.Log("Puzzle Solved!" );
            }
        }
        else
        {
            Debug.Log("Incorrect Runestone. Resetting Puzzle.");
            ResetPuzzle();
        }
    }

    // Reset the puzzle if the player makes a mistake
    private void ResetPuzzle()
    {
        currentOrder = 0;
        // Optional: Reset visual states of runestones here
    }
}
