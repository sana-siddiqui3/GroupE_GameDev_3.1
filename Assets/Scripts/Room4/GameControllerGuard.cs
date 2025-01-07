using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameControllerRoom4 : MonoBehaviour
{
    public Camera playerView;
    public Camera fightView;
    public GameObject FightUI;

    [SerializeField] private GameObject Player = null;
    [SerializeField] private GameObject Enemy = null;
    [SerializeField] private GameObject Enemy2 = null;

    [SerializeField] private Slider PlayerHealth = null;
    [SerializeField] private Slider EnemyHealth = null;
    [SerializeField] private Slider Enemy2Health = null;

    [SerializeField] private Button enemy1Button = null;
    [SerializeField] private Button enemy2Button = null;

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
    public Transform playerFightPosition;
    public Transform enemyFightPosition;
    public Transform enemyFightPosition2;
    private bool isVictoryAchieved = false;

    [SerializeField] private GuardController enemyGhost1Controller;
    [SerializeField] private GuardController enemyGhost2Controller;

    private GameObject currentTarget = null;
    public bool isFightActive = false;

    public void Start()
    {
        FightUI.SetActive(false);
        UpdateEnergyUI();
        InitializeDeck();
        PlayerData.instance.setObjective("Defeat the guards.");

        if (PlayerData.instance != null)
        {
            PlayerHealth.maxValue = 100;
            PlayerHealth.value = PlayerData.instance.playerHealth;
        }

        // Assign enemy buttons
        enemy1Button.onClick.AddListener(() => SetTarget(Enemy, EnemyHealth));
        enemy2Button.onClick.AddListener(() => SetTarget(Enemy2, Enemy2Health));
    }

    public void Update()
    {
        PlayerHealth.value = PlayerData.instance.playerHealth;
    }

    public void StartFight()
{
    FightUI.SetActive(true);

    // Set up positions and cameras
    Player.transform.position = playerFightPosition.position;
    Player.transform.rotation = playerFightPosition.rotation;

    Enemy.transform.position = enemyFightPosition.position;
    Enemy.transform.rotation = enemyFightPosition.rotation;

    Enemy2.transform.position = enemyFightPosition2.position;
    Enemy2.transform.rotation = enemyFightPosition2.rotation;

    enemyGhost1Controller.hasStartedFight = true;
    enemyGhost2Controller.hasStartedFight = true;

    playerView.enabled = false;
    fightView.enabled = true;

    InitializeDeck();
    DrawCards(5);

    isFightActive = true;
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

            if (card == "Heal Card" || card == "Shield Card" || card == "AttackAll Card" ||  currentTarget != null ||card == "Energy Card"){

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
                Debug.Log("Not enough energy to play this card / Enemy not selected.");
            }
        }
    }

    private void SetTarget(GameObject target, Slider targetHealth)
    {
        if (target == Enemy && enemyGhost1Controller.isEnemyDefeated)
        {
            Debug.Log("Enemy 1 is already defeated and cannot be targeted.");
            return;  // Do nothing if the enemy is defeated
        }

        if (target == Enemy2 && enemyGhost2Controller.isEnemyDefeated)
        {
            Debug.Log("Enemy 2 is already defeated and cannot be targeted.");
            return;  // Do nothing if the enemy is defeated
        }

        currentTarget = target;
        Debug.Log($"{target.name} selected!");
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
            // Only attack if a valid target is selected
            if (currentTarget != null)
            {
                Attack(currentTarget, 10 * multiplier1); 
            }
            else
            {
                Debug.Log("No target selected. Cannot attack.");
            }
        }
        else if (card == "Heal Card")
        {
            // Always heal the player
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
            if (currentTarget != null)
            {
                Attack(currentTarget, 5 * multiplier2);
                Heal(Player, 5 * multiplier2);
            }
            else
            {
                Debug.Log("No target selected. Cannot attack.");
            }
        }
        else if (card == "TripleAttack Card")
        {
            if (currentTarget != null)
            {
                Attack(currentTarget, 30 * multiplier1);
            }
            else
            {
                Debug.Log("No target selected. Cannot attack.");
            }
        }
        else if (card == "AttackAll Card")
        {
            Attack(Enemy, 10 * multiplier1);
            Attack(Enemy2, 10 * multiplier1);
            
        }

        else if (card == "BadAttack Card")
        {
            if (currentTarget != null)
            {
                Attack(currentTarget, 5 * multiplier2);
            }
            else
            {
                Debug.Log("No target selected. Cannot attack.");
            }
        }
        else if (card == "LowAttack Card")
        {
            if (currentTarget != null)
            {
                Attack(currentTarget, 2 * multiplier3);
            }
            else
            {
                Debug.Log("No target selected. Cannot attack.");
            }
        }


        DisplayCardsInFightUI(); // Refresh the card UI after the action
    }

    private void EndTurn()
    {
        ResetTurn();
        discardPile.AddRange(drawnCards);
        LogDeckAndDiscardState();
        changeTurn();
    }

    private void ResetTurn()
    {
        selectedCards.Clear();
        cardsSelected = 0;
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

        Debug.Log("Enemy Turn Begins");

        // Check if Enemy 1 is alive, and then perform its actions
        if (EnemyHealth.value > 0)
        {
            Debug.Log("Enemy 1's Turn - Attacking/Healing Player");
            EnemyAction(Enemy);
        }
        else
        {
            Debug.Log("Enemy 1 is defeated and will not act.");
        }

        // Check if Enemy 2 is alive, and then perform its actions
        if (Enemy2Health.value > 0)
        {
            Debug.Log("Enemy 2's Turn - Attacking/Healing Player");
            EnemyAction(Enemy2);
        }
        else
        {
            Debug.Log("Enemy 2 is defeated and will not act.");
        }

        // Wait for a short duration before ending the turn
        yield return new WaitForSeconds(1); // Adjust the wait time if needed

        // Reset energy for the player's turn
        currentEnergy = maxEnergy;
        UpdateEnergyUI();
        changeTurn();
    }

    // Method to handle enemy actions (both attacking and healing the player)
    private void EnemyAction(GameObject enemy)
    {
        int random = Random.Range(1, 3);  // Randomly choose an action: attack or heal

        if (random == 1)
        {
            Attack(Player, 4);  // Perform attack on player
            Heal(enemy, 5);      // Heal the enemy
        }
        else
        {
            Attack(Player, 5);  // Perform attack on player
            Heal(enemy, 10);     // Heal the enemy
        }
    }

    public void Attack(GameObject target, float damage)
    {
        if (target == Enemy && !enemyGhost1Controller.isEnemyDefeated)
        {
            EnemyHealth.value -= damage;
            if (EnemyHealth.value <= 0)
            {
                enemyGhost1Controller.isEnemyDefeated = true;  // Mark enemy 1 as defeated
                FallOver(target);
                enemy1Button.interactable = false;  // Disable the attack button for this enemy
                currentTarget = null;  // Deselect the current target
                Debug.Log("Enemy 1 defeated and deselected!");
            }
        }
        else if (target == Enemy2 && !enemyGhost2Controller.isEnemyDefeated)
        {
            Enemy2Health.value -= damage;
            if (Enemy2Health.value <= 0)
            {
                enemyGhost2Controller.isEnemyDefeated = true;  // Mark enemy 2 as defeated
                FallOver(target);
                enemy2Button.interactable = false;  // Disable the attack button for this enemy
                currentTarget = null;  // Deselect the current target
                Debug.Log("Enemy 2 defeated and deselected!");
            }
        }
        else
        {
            PlayerHealth.value -= damage;

            if (PlayerData.instance != null)
                PlayerData.instance.DamagePlayer(damage);

            if (PlayerHealth.value <= 0)
            {
                FallOver(target);
                GameOver(); // Trigger the game over when player health is zero or less
            }
        }
    }

    public void Heal(GameObject target, float amount)
    {
        if (target == Enemy)
        {
            EnemyHealth.value += amount;
        }
        else if (target == Enemy2)
        {
            Enemy2Health.value += amount;
        }
        else
        {
            PlayerHealth.value += amount;

            if (PlayerData.instance != null)
                PlayerData.instance.HealPlayer(amount);
        }
    }

    public void UsePoisonPotion()
    {
        // Check if poison can only be used in guard fight
        if (!isFightActive )
        {
            Debug.Log("The poison potion cannot be used in this fight!");
            return;
        }

        // Mark enemies as defeated when poison is used
        enemyGhost1Controller.isEnemyDefeated = true;
        enemyGhost2Controller.isEnemyDefeated = true;

        EnemyHealth.value = 0;
        Enemy2Health.value = 0;

        resultText.text = "You used poison to skip the battle!";
        CheckForVictory();

        // Remove the Poison Potion from the inventory
        if (PlayerData.instance != null)
        {
            PlayerData.instance.RemoveItem("Poison Potion");
            InventoryTooltip.instance.gameObject.SetActive(false);

            PlayerInventory playerInventory = FindFirstObjectByType<PlayerInventory>();
            if (playerInventory != null)
            {
                playerInventory.UpdateInventoryDisplay();
            }
        }
    }


    private void FallOver(GameObject target)
    {
        target.transform.Rotate(new Vector3(90f, 0f, 0f)); // Simulate falling over

        if (target == Enemy)
        {
            Debug.Log("Enemy 1 defeated!");
        }
        else if (target == Enemy2)
        {
            Debug.Log("Enemy 2 defeated!");
        }

        CheckForVictory();
    }

    private void CheckForVictory()
    {
        if (EnemyHealth.value <= 0 && Enemy2Health.value <= 0)
        {
            isVictoryAchieved = true; // Set the victory flag
            isGameOver = true;
            resultText.text = "You Win!";

            // Save player's health before ending the fight
            if (PlayerData.instance != null)
            {
                PlayerData.instance.playerHealth = PlayerHealth.value;
            }

            // Switch back to player view and end the fight
            fightView.enabled = false;
            playerView.enabled = true;
            FightUI.SetActive(false);
            isFightActive = false; // Mark the fight as inactive

            Debug.Log("Both enemies are defeated. Game Over!");
        }
    }

    public bool IsVictoryAchieved()
    {
        return isVictoryAchieved;
    }

    private void GameOver()
    {
        isGameOver = true;
        isFightActive = false; // Mark the fight as inactive
        resultText.text = "Game Over! You Lose.";
        Debug.Log("Player defeated. Game Over!");
        PlayerData.instance.ResetPlayerData();
        SceneManager.LoadScene("MainMenu");
    }

    private void UpdateEnergyUI()
    {
        energyText.text = $"Energy: {currentEnergy}/{maxEnergy}";
    }

    private void LogDeckAndDiscardState()
    {
        Debug.Log($"Deck: {string.Join(", ", deck)}");
        Debug.Log($"Discard Pile: {string.Join(", ", discardPile)}");
    }
}
