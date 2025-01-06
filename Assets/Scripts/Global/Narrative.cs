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
        { "Room1", "You awaken to the cold, damp air of a haunted prison cell. The harsh walls are made of ancient stone, and the dim light from a flickering torch barely illuminates the room. Your hands are shackled, and the only sound is the distant echo of unknown footsteps. You can feel the weight of despair hanging in the air, but there is something stronger inside you—a will to escape. The door to exit is locked, and your only hope for escape lies with the skeleton nearby. It rests inside an old, weathered chest. The chest is locked, but inside it lies the key to your freedom. The key to the door.You can either sit here and wait for your fate or push forward and find a way to unlock the chest. The path to freedom starts here. Every step will bring you closer to escaping this forsaken prison. But be warned—danger lurks around every corner, and you’re not the only one imprisoned here." },
    
    {"Room2", "The heavy metal door creaks open with a faint, sinister sound. You’ve managed to escape your cell, but the real challenge begins now. The hallways of the prison stretch ahead of you, dark and claustrophobic. The walls seem to close in, and the air is thick with the stench of old stone and rot. A distant sound of a guard’s footsteps echoes, but they fade quickly. You have no time to waste. You can see the faint outlines of other cells, some of them empty, some with shadows of long-forgotten prisoners staring blankly into the void. The place feels abandoned, but it’s not. There are still dangers here. The guard patrols are erratic, and there may be traps that you cannot see—silent threats that only reveal themselves when it’s too late. Ahead, you notice a barred door. It’s locked, but there’s a way to bypass it. You need to make a choice. Do you risk trying to find the key, or do you push forward into the next room? Every decision will affect your path to freedom, and every mistake could be your last. The deeper you go into this cursed place, the closer you get to what lies at the heart of the prison. Prepare yourself. The real tests begin now."},
    
    {"Room3", "The narrow corridors have led you here, deeper into the heart of the prison. The air feels colder, and the walls are more worn than ever. You’ve made it further than most who tried to escape, but now, with every step, you can feel the weight of the place bearing down on you. There’s something about this room—an ominous presence that you can’t shake. The stone seems to pulse with a dark energy. You glance around and spot several oddities—strange markings on the floor, old bloodstains that suggest something terrible happened here long ago. You can hear the faint echoes of voices, too distant to understand, but enough to unsettle you. Suddenly, the door slams shut behind you. There’s no turning back now. You must face the prison’s true horrors. A group of enemies appears, their faces twisted with madness. They’ve been trapped here longer than you, and they’ve grown desperate. The only way to proceed is to fight your way through, but you must be careful. These enemies are vicious, and they won’t hesitate to take you down. The exit is close, but so is your worst nightmare. Will you find the strength to face these foes? Or will the prison claim you as yet another lost soul?"},
    
    {"Room4", "You’ve reached the final room, and the end is in sight. But the prison is not finished with you yet. The air here is thick with tension, and the room feels suffocating. The stone walls seem to close in, and the faintest noise echoes endlessly, creating an oppressive sense of dread. At the far end of the room, you see it—the exit. But standing between you and freedom is a towering figure, cloaked in darkness. It’s the final guardian of the prison, a twisted and corrupted soul bound to protect the prison’s secrets. This guardian is the one who keeps the prisoners from escaping, and it will not let you leave without a fight. The final battle awaits, and it will not be easy. The guardian is powerful, its strikes heavy and brutal. You must use everything you’ve learned so far, every skill, every strategy, to overcome this nightmare. But remember, the guardian feeds on fear, and the more you hesitate, the more powerful it becomes. This is your last chance. Defeat the guardian, escape the prison, and put an end to this nightmare once and for all. The final test has begun, and only one will walk away from this cursed place."}
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
