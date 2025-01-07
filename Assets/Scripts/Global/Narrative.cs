using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class NarrativeManager : MonoBehaviour
{
    public GameObject blackScreen;          // Reference to the black screen panel
    public TextMeshProUGUI narrativeText;   // Reference to the narrative text component
    public GameObject hudUI;               // Reference to the HUD UI (health, inventory, etc.)
    private bool narrativeStarted = false;  // To track if narrative has started

    // Dictionary to store different narratives for each scene
    private Dictionary<string, string> levelNarratives = new Dictionary<string, string>
    {
        { "Room1", "You awaken in a dark, damp cell. The air is cold, and despair lingers in the shadows. A locked door bars your escape, but a nearby skeleton clutches a chest. Inside lies the key to your freedom. Begin your journey to escape this cursed prison, but beware—the dangers ahead are relentless." },
        { "Room2", "You made it out the first room, but your trials are far from over. The haunted corridor stretches ahead, its shadows hiding patrolling ghosts. Choose wisely: risk the perilous glass bridge for treasures or take the safer path and press on. Each step brings you closer to freedom or despair." },
        { "Room3", "You enter the cursed chamber, the air thick with an eerie chill. Zombies shamble in the darkness, guarding the corrupted heart crystal. Retrieve it to proceed, but beware—treasures and traps alike are hidden here. Find the purification potion to lift the curse and advance." },
        { "Room4", "The final chamber awaits, suffused with a heavy dread. Guards stand ready, blocking the path to the lair of the Corrupted Guardian. Defeat them to gain access. This is your last chance to escape—prepare for the ultimate battle." }
    };

    private bool enterPressed = false; // To track whether Enter has been pressed
    private float enterCooldownTime = 1;
    private float waitTime = 0; // Variable to track the wait time

    void Start()
    {
        blackScreen.SetActive(false); // Initially hide the black screen
        if (hudUI != null)
        {
            hudUI.SetActive(true); // Ensure HUD is visible at the start
        }

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
        // Hide the HUD UI when showing the narrative
        if (hudUI != null)
        {
            hudUI.SetActive(false);
        }

        // Show the black screen and the text
        blackScreen.SetActive(true);
        narrativeText.gameObject.SetActive(true);

        // Start typing out the narrative text
        narrativeText.text = ""; // Clear the text before starting
        float typingSpeed = 0.05f; // Adjust speed here

        bool skipText = false;

        foreach (char letter in narrative)
        {
            // Handle the Enter key press during typing (to skip typing)
            if (UnityEngine.InputSystem.Keyboard.current.enterKey.wasPressedThisFrame && !enterPressed)
            {
                skipText = true;
                enterPressed = true; // Set the flag that Enter was pressed
                break;
            }

            narrativeText.text += letter;
            yield return new WaitForSeconds(typingSpeed); // Wait for the specified speed
        }

        // If Enter was pressed during typing, instantly finish typing the rest
        if (skipText)
        {
            narrativeText.text = narrative;
        }

        // Wait for the player to press a key to skip or wait for a few seconds (optional)
        bool waitingForInput = true;
        while (waitingForInput)
        {
            // Reset the Enter key detection after the cooldown period
            if (enterPressed && enterCooldownTime <= 0)
            {
                enterPressed = false; // Reset flag after cooldown
            }

            // Check for Enter key press to continue
            if (UnityEngine.InputSystem.Keyboard.current.enterKey.wasPressedThisFrame && !enterPressed)
            {
                waitingForInput = false;
                enterPressed = true; // Set flag when Enter is pressed
            }
            // Check if 3 seconds have passed
            else if (waitTime >= 3f)
            {
                waitingForInput = false;
            }
            else
            {
                waitTime += Time.deltaTime;
                enterCooldownTime -= Time.deltaTime;
                yield return null;
            }
        }

        // After typing is complete or key is pressed, hide the narrative and start the level
        narrativeText.gameObject.SetActive(false);
        blackScreen.SetActive(false); // Hide black screen

        // Show the HUD UI again after the narrative
        if (hudUI != null)
        {
            hudUI.SetActive(true);
        }

        StartLevel();
    }

    private void StartLevel()
    {
        // Trigger the level start logic here (e.g., enable the player to move, spawn enemies, etc.)
        Debug.Log("Level has started!");
    }
}
