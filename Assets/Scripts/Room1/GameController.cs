using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
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
    private EnemyController enemyController; // Reference to the EnemyController script

    public void Start()
    {
        FightUI.SetActive(false);
        UpdateEnergyUI();
        InitializeDeck();

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
        enemyController = enemy.GetComponent<EnemyController>(); // Get the EnemyController component
        if (enemyController != null)
        {
            enemyController.StopEnemy(); // Stop the enemy's movement
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

    public void DisplayCardsInFightUI()
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

        // Retrieve card details dynamically (replace with your card system logic)
        int energyCost = GetCardEnergyCost(card);
        int attackAmount = GetCardAttackAmount(card);

        cardUIScript.SetCardDetails(card, energyCost, attackAmount);

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
        case "Heal Card": return 2;
        default: return 1;
    }
}

private int GetCardAttackAmount(string card)
{
    // Example logic to retrieve attack amount (replace with actual card data retrieval)
    switch (card)
    {
        case "Attack Card": return 5;
        case "Heal Card": return 0;
        default: return 1;
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
            Heal(Player, 5 * multiplier2);
        }
        else if (card == "AttackBlock Card")
        {
            Attack(Enemy, 5 * multiplier2);
            Heal(Player, 5  * multiplier2);
        }
        else if (card == "TripleAttack Card")
        {
            Attack(Enemy, 30 * multiplier1);
        }
        else if (card == "AttackAll Card")
        {
            Attack(Enemy, 10 * multiplier1); ;
        }
        else if (card == "BadAttack Card")
        {
            Attack(Enemy, 8 * multiplier1);
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
            currentEnergy = 3;
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
            Attack(Player, 20);
            Heal(Enemy, 5);
        }
        else
        {
            Attack(Player, 10);
            Heal(Enemy, 10);
        }

        currentEnergy = 3;
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
            {
                PlayerData.instance.HealPlayer(amount); // Save updated health
            }
        }
    }

    public void FallOver(GameObject target)
{
    target.transform.Rotate(new Vector3(90f, 0f, 0f));
    isGameOver = true;

    if (target == Enemy)
    {
        resultText.text = "You Win!";
        fightView.enabled = false;
        playerView.enabled = true;
        FightUI.SetActive(false);
        Enemy.GetComponent<EnemyController>().isEnemyDefeated = true;
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
        energyText.text = $"Energy: {currentEnergy}/{3}";
    }

    // Debugging: Log deck and discard pile state at the end of each turn
    private void LogDeckAndDiscardState()
    {
        Debug.Log($"Deck: {string.Join(", ", deck)}");
        Debug.Log($"Discard Pile: {string.Join(", ", discardPile)}");
    }
}
