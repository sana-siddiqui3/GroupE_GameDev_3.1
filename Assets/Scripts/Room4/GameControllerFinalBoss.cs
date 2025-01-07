using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameControllerFinalBoss : MonoBehaviour
{
    public Camera playerView;
    public Camera fightView;
    public GameObject FightUI;

    [SerializeField] private GameObject Player = null;
    [SerializeField] private GameObject Enemy = null;

    [SerializeField] private Slider PlayerHealth = null;
    [SerializeField] private Slider EnemyHealth = null;
    public GameObject cardUIPrefab;
    public GameObject cardPanel;

    [SerializeField] private TextMeshProUGUI resultText = null;
    [SerializeField] private TextMeshProUGUI energyText = null;

    private bool isPlayerTurn = true;
    private bool isGameOver = false;

    private int currentEnergy = 3;
    private const int maxEnergy = 3;

    private List<string> deck = new List<string>();
    private List<string> discardPile = new List<string>();
    private List<string> drawnCards = new List<string>();

    private int cardsSelected = 0;
    private List<string> selectedCards = new List<string>();
    private List<Button> cardButtons = new List<Button>();
    [SerializeField] private Button endTurnButton = null;

    public GameObject fightPromptUI; // UI element that shows when the player can fight
    public GameObject fightUI;      // UI element for the fight screen
    public GameObject player;       // Reference to the player object
    public GameObject enemy;        // Reference to the enemy object
    public Transform playerFightPosition; // Predefined player position for the fight
    public Transform enemyFightPosition;  // Predefined enemy position for the fight
    private FinalBossController enemyController; // Reference to the EnemyController script


    public void Start()
    {
        FightUI.SetActive(false);
        UpdateEnergyUI();
        InitializeDeck();
        PlayerData.instance.setObjective("Defeat the boss to win the game!");

        // Load player health from PlayerData
        if (PlayerData.instance != null)
        {
            PlayerHealth.maxValue = 100; // Set max health
            PlayerHealth.value = PlayerData.instance.playerHealth; // Set current health
        }
    }

    public void Update()
    {
        PlayerHealth.value = PlayerData.instance.playerHealth;
    }

    public void StartFight()
    {
        // Hide the fight prompt and show the fight UI
        fightPromptUI.SetActive(false);
        fightUI.SetActive(true);

        // Set the player to the fight position
        player.transform.position = playerFightPosition.position;
        player.transform.rotation = playerFightPosition.rotation;

        enemy.transform.position = enemyFightPosition.position;
        enemy.transform.rotation = enemyFightPosition.rotation;

        // Switch cameras to the fight view
        playerView.enabled = false;
        fightView.enabled = true;

        // Stop enemy movement
        enemyController = enemy.GetComponent<FinalBossController>(); // Get the EnemyController component
        if (enemyController != null)
        {
            enemyController.StopEnemy(); // Stop the enemy's movement
        }

        // Load player health from PlayerData without resetting
        if (PlayerData.instance != null)
        {
            PlayerHealth.value = PlayerData.instance.playerHealth; // Set current health
        }

        FightUI.SetActive(true); // Activate the fight UI in the GameController
        InitializeDeck();
        DrawCards(5);
    }

    public void InitializeDeck()
    {
        if (PlayerData.instance != null)
        {
            foreach(InventoryItem item in PlayerData.instance.inventory)
            {
                if(item.itemName.Contains("Card"))
                {
                    Debug.Log("Adding card to deck: " + item.itemName);
                    deck.Add(item.itemName);
                }
            }
        }

        ShuffleDeck();
    }

    private void ShuffleDeck()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            string temp = deck[i];
            int randomIndex = Random.Range(0, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

    public void DrawCards(int count)
    {
        drawnCards.Clear();

        for (int i = 0; i < count; i++)
        {
            if (deck.Count == 0)
            {
                deck.AddRange(discardPile);
                discardPile.Clear();
                ShuffleDeck();
            }

            if (deck.Count > 0)
            {
                string drawnCard = deck[0];
                drawnCards.Add(drawnCard);
                deck.RemoveAt(0);
            }
        }

        DisplayCardsInFightUI();
    }

    private void DisplayCardsInFightUI()
{
    foreach (Transform child in cardPanel.transform)
    {
        Destroy(child.gameObject);
    }

    cardButtons.Clear();

    foreach (string card in drawnCards)
    {
        GameObject cardUI = Instantiate(cardUIPrefab, cardPanel.transform);
        CardUI cardUIScript = cardUI.GetComponent<CardUI>();

        // Set the card details (name, energy cost, attack amount)
        int energyCost = GetCardEnergyCost(card); // Example: Each card costs 2 energy (you can set this dynamically)
        int attackAmount = GetCardAmount(card); // Get the attack amount based on the card and difficulty

        cardUIScript.SetCardDetails(card, energyCost, attackAmount); // Set the card details

        Button cardButton = cardUI.GetComponent<Button>();
        cardButtons.Add(cardButton);

        cardButton.onClick.AddListener(() => SelectCard(card));
    }
}

private int GetCardEnergyCost(string card)
{
    // Example logic to retrieve energy cost (replace with actual card data retrieval)
    switch (card)
    {
        case "Attack Card": return 1;
        case "Heal Card": return 1;
        case "Energy Card": return 0;
        case "Shield Card": return 1;
        case "AttackBlock Card": return 1;
        case "TripleAttack Card": return 1;
        case "AttackAll Card": return 1;
        case "BadAttack Card": return 2;
        case "LowAttack Card": return 1;
        default: return 1;
    }
}

    private int GetCardAmount(string card)
{
    float multiplier1 = 1.0f;
    float multiplier2 = 1.0f;
    float multiplier3 = 1.0f;

    // Difficulty multipliers
    switch (PlayerPrefs.GetInt("Difficulty", 1)) // Default difficulty: 1 (Normal)
    {
        case 0: // Easy
            multiplier1 = 1.5f; // Increase card effects
            multiplier2 = 2f;
            multiplier3 = 2f;
            break;
        case 1: // Normal
            multiplier1 = 1.0f;
            multiplier2 = 1.0f;
            multiplier3 = 1.0f;
            break;
        case 2: // Hard
            multiplier1 = 0.5f; // Decrease card effects
            multiplier2 = 0.4f;
            multiplier3 = 0f;
            break;
    }

    // Example logic to retrieve card amount (replace with actual card data retrieval)
    switch (card)
    {
        case "Attack Card": return (int)(10 * multiplier1);
        case "Heal Card": return (int)(10 * multiplier1);
        case "Energy Card": return 1;
        case "Shield Card": return (int)(5 * multiplier2);
        case "AttackBlock Card": return (int)(5 * multiplier2);
        case "TripleAttack Card": return (int)(30 * multiplier1);
        case "AttackAll Card": return (int)(10 * multiplier1);
        case "BadAttack Card": return (int)(5 * multiplier2);
        case "LowAttack Card": return (int)(2 * multiplier3);
        default: return 0;
    }
}

    private void SelectCard(string card)
{
    // Only allow card selection if it doesn't cause the energy to go below 0
    if (card == "Energy Card" || currentEnergy > 0)  // Allow Energy Card and cards that don't reduce energy
    {
        // Check energy requirements before playing the card
        bool canPlayCard = false;

        if (card == "TripleAttack Card" && currentEnergy >= 3)
        {
            canPlayCard = true;
        }
        else if (card == "BadAttack Card" && currentEnergy >= 2)
        {
            canPlayCard = true;
        }
        else if (card != "TripleAttack Card" && card != "BadAttack Card") // For other cards
        {
            canPlayCard = true;
        }

        // If the card can be played, update the energy and select it
        if (canPlayCard)
        {
            selectedCards.Add(card);
            cardsSelected++;

            // Deduct energy
            if (card == "TripleAttack Card")
            {
                currentEnergy -= 3;
            }
            else if (card == "BadAttack Card")
            {
                currentEnergy -= 2;
            }
            else if (card != "Energy Card")
            {
                currentEnergy--;
            }

            // Update the energy UI
            UpdateEnergyUI();

            // Move the card to the discard pile and remove it from the drawn cards
            discardPile.Add(card);
            drawnCards.Remove(card);

            // Apply the effect of the card
            ApplyCardEffect(card);
        }
        else
        {
            Debug.Log("Not enough energy to play this card.");
        }
    }
}

    public void PlayerEndTurn()
    {
        if (isPlayerTurn && !isGameOver)
        {
            EndTurn();
        }
    }

    private void ApplyCardEffect(string card)
    {
        float multiplier1 = 1.0f;
        float multiplier2 = 1.0f;
        float multiplier3 = 1.0f;
        switch (PlayerPrefs.GetInt("Difficulty", 1)) // Default difficulty: 1 (Normal)
        {
            case 0: // Easy
                multiplier1 = 1.5f; // Increase card effects
                multiplier2 = 2f; // Increase card effects
                multiplier3 = 2f;
                break;
            case 1: // Normal
                multiplier1 = 1.0f; // Default
                multiplier2 = 1.0f; // Default
                multiplier3 = 1.0f;
                break;
            case 2: // Hard
                multiplier1 = 0.5f; // Decrease card effects
                multiplier2 = 0.4f; // Decrease card effects
                multiplier3 = 0f; 
                break;
        }

        if (card == "Attack Card")
        {
            Attack(Enemy, 10 * multiplier1);
        }
        else if (card == "Heal Card")
        {
            Heal(Player, 10 * multiplier1);
        } 
        else if (card == "Energy Card")
        {
            currentEnergy++;
            UpdateEnergyUI();
        }
        else if (card == "Shield Card")
        {
            Heal(Player, 5 * multiplier2); ;
        }
        else if (card == "AttackBlock Card")
        {
            Attack(Enemy, 5 * multiplier2);
            Heal(Player, 5 * multiplier2);
        }
        else if (card == "TripleAttack Card")
        {
            Attack(Enemy, 30 * multiplier1);
        }
        else if (card == "AttackAll Card")
        {
            Attack(Enemy, 10 * multiplier1);
        }
        else if (card == "BadAttack Card")
        {
            Attack(Enemy, 5 * multiplier2);
        }
        else if (card == "LowAttack Card")
        {
            Attack(Enemy, 2 * multiplier3);
        }

        DisplayCardsInFightUI();  // Refresh the UI
    }

    private void ResetTurn()
    {
        selectedCards.Clear();
        cardsSelected = 0;
    }

    private void EndTurn()
    {
        ResetTurn();
        discardPile.AddRange(drawnCards);  // Add all drawn cards to the discard pile
        LogDeckAndDiscardState();
        changeTurn();
    }

    private void changeTurn()
    {
        if (isGameOver) return;

        isPlayerTurn = !isPlayerTurn;

        endTurnButton.interactable = isPlayerTurn;

        if (!isPlayerTurn)
        {
            StartCoroutine(EnemyTurn());
        }
        else
        {
            currentEnergy = maxEnergy;
            UpdateEnergyUI();
            DrawCards(5);
        }
    }

    private IEnumerator EnemyTurn()
    {
        if (isGameOver) yield break;

        yield return new WaitForSeconds(3);

        int random = Random.Range(1, 3);

        if (random == 1)
        {
            Attack(Player, 10);
            Heal(Enemy, 10);
        }
        else
        {
            Attack(Player, 8);
            Heal(Enemy, 7);
        }

        currentEnergy = maxEnergy;
        UpdateEnergyUI();
        changeTurn();
    }

    public void Attack(GameObject target, float damage)
    {
        if (target == Enemy)
        {
            EnemyHealth.value -= damage;
            if (EnemyHealth.value <= 0)
            {
                EnemyHealth.value = 0;
                FallOver(target);
            }
        }
        else
        {
            PlayerHealth.value -= damage;

            if (PlayerData.instance != null)
            {
                PlayerData.instance.DamagePlayer(damage); // Save updated health
            }

            if (PlayerHealth.value <= 0)
            {
                PlayerHealth.value = 0;
                FallOver(target);
            }
        }
    }

    public void Heal(GameObject target, float amount)
    {
        if (target == Enemy)
        {
            EnemyHealth.value += amount;
        }
        else
        {
            PlayerHealth.value += amount;

            if (PlayerData.instance != null)
                PlayerData.instance.HealPlayer(amount);
        }
    }

    private void FallOver(GameObject target)
    {
        target.transform.Rotate(new Vector3(90f, 0f, 0f));
        isGameOver = true;

        if (target == Enemy)
        {
            resultText.text = "You Win!";
            fightView.enabled = false;
            playerView.enabled = true;
            FightUI.SetActive(false);
            Enemy.GetComponent<FinalBossController>().isEnemyDefeated = true;
            StartCoroutine(ShowEnding());
        }
        else if (target == Player)
        {
            resultText.text = "You Lose!";
            PlayerData.instance.ResetPlayerData();
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void UpdateEnergyUI()
    {
        energyText.text = $"Energy: {currentEnergy}/{maxEnergy}";
    }

    // Debugging: Log deck and discard pile state at the end of each turn
    private void LogDeckAndDiscardState()
    {
        Debug.Log($"Deck: {string.Join(", ", deck)}");
        Debug.Log($"Discard Pile: {string.Join(", ", discardPile)}");
    }

    private IEnumerator ShowEnding()
{
    // Display a white screen with a message
    GameObject endingCanvas = new GameObject("EndingCanvas");
    Canvas canvas = endingCanvas.AddComponent<Canvas>();
    canvas.renderMode = RenderMode.ScreenSpaceOverlay;
    endingCanvas.AddComponent<CanvasScaler>();
    endingCanvas.AddComponent<GraphicRaycaster>();

    // Create a white background
    GameObject whiteBackground = new GameObject("WhiteBackground");
    whiteBackground.transform.parent = endingCanvas.transform;
    Image bgImage = whiteBackground.AddComponent<Image>();
    bgImage.color = Color.white; // Set the background color to white
    RectTransform bgTransform = whiteBackground.GetComponent<RectTransform>();
    bgTransform.anchorMin = Vector2.zero;
    bgTransform.anchorMax = Vector2.one;
    bgTransform.offsetMin = Vector2.zero;
    bgTransform.offsetMax = Vector2.zero;

    // Create a text element
    GameObject congratsText = new GameObject("CongratsText");
    congratsText.transform.parent = endingCanvas.transform;
    TextMeshProUGUI text = congratsText.AddComponent<TextMeshProUGUI>();
    text.text = "Congratulations! You Escaped!";
    text.alignment = TextAlignmentOptions.Center;
    text.fontSize = 36;
    text.color = Color.black;

    RectTransform textTransform = congratsText.GetComponent<RectTransform>();
    textTransform.anchorMin = new Vector2(0.5f, 0.5f);
    textTransform.anchorMax = new Vector2(0.5f, 0.5f);
    textTransform.anchoredPosition = Vector2.zero;

    // Wait for a few seconds before returning to the main menu
    yield return new WaitForSeconds(3);
    PlayerData.instance.ResetPlayerData();
    SceneManager.LoadScene("MainMenu");
}
}
