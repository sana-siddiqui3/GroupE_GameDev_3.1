using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class NarrativeManager : MonoBehaviour
{
    public GameObject blackScreen;          // Reference to the black screen panel
    public TextMeshProUGUI narrativeText;   // Reference to the narrative text component
    private bool narrativeStarted = false;  // To track if narrative has started

    // Dictionary to store different narratives for each scene
    private Dictionary<string, string> levelNarratives = new Dictionary<string, string>
    {
        { "Room1", "You awaken in a dark, damp cell. The air is cold, and despair lingers in the shadows. A locked door bars your escape, but a nearby skeleton clutches a chest. Inside lies the key to your freedom. Begin your journey to escape this cursed prison, but beware—the dangers ahead are relentless." },
        { "Room2", "You made it out the first room, but your trials are far from over. The haunted corridor stretches ahead, its shadows hiding patrolling ghosts. Choose wisely: risk the perilous glass bridge for treasures or take the safer path and press on. Each step brings you closer to freedom or despair." },
        { "Room3", "You enter the cursed chamber, the air thick with an eerie chill. Zombies shamble in the darkness, guarding the corrupted heart crystal. Retrieve it to proceed, but beware—treasures and traps alike are hidden here. Find the purification potion to lift the curse and advance." },
        { "Room4", "The final chamber awaits, suffused with a heavy dread. Guards stand ready, blocking the path to the lair of the Corrupted Guardian. Defeat them to gain access. This is your last chance to escape—prepare for the ultimate battle." }
       
    };

    void Start()
    {
        blackScreen.SetActive(false); // Initially hide the black screen
        if (!narrativeStarted)
        {
            string currentScene = SceneManager.GetActiveScene().name;
            if (levelNarratives.ContainsKey(currentScene))
            {
                StartNarrative(levelNarratives[currentScene]);
            }
            else
            {
                Debug.LogError("Narrative for this level is missing!");
            }
            narrativeStarted = true; // Ensure narrative only starts once
        }
    }

    public void StartNarrative(string narrative)
    {
        StartCoroutine(DisplayNarrative(narrative));
    }

    private IEnumerator DisplayNarrative(string narrative)
    {
        // Show the black screen and the text
        blackScreen.SetActive(true);
        narrativeText.gameObject.SetActive(true);

        // Start typing out the narrative text
        narrativeText.text = ""; // Clear the text before starting
        foreach (char letter in narrative)
        {
            narrativeText.text += letter;
            yield return new WaitForSeconds(0.05f); // Adjust speed here
        }

        // Wait for the player to press a key to skip or wait for a few seconds (optional)
        float waitTime = 0;
        while (waitTime < 5f && !UnityEngine.InputSystem.Keyboard.current.enterKey.wasPressedThisFrame)
        {
            waitTime += Time.deltaTime;
            yield return null;
        }

        // After typing is complete or key is pressed, hide the narrative and start the level
        narrativeText.gameObject.SetActive(false);
        blackScreen.SetActive(false); // Hide black screen
        StartLevel();
    }

    private void StartLevel()
    {
        // Trigger the level start logic here (e.g., enable the player to move, spawn enemies, etc.)
        Debug.Log("Level has started!");
    }
}
