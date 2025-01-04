using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameControllerGuard : MonoBehaviour
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

    [SerializeField] private GuardController enemyGuard1Controller;
    [SerializeField] private GuardController enemyGuard2Controller;

    private GameObject currentTarget = null;

    public void Start()
    {
        FightUI.SetActive(false);
        UpdateEnergyUI();
        InitializeDeck();

        if (PlayerData.instance != null)
        {
            PlayerHealth.maxValue = 100;
            PlayerHealth.value = PlayerData.instance.playerHealth;
        }

        // Assign enemy buttons
        enemy1Button.onClick.AddListener(() => SetTarget(Enemy, EnemyHealth));
        enemy2Button.onClick.AddListener(() => SetTarget(Enemy2, Enemy2Health));
    }

    public void StartFight()
    {
        FightUI.SetActive(true);

        Player.transform.position = playerFightPosition.position;
        Player.transform.rotation = playerFightPosition.rotation;

        Enemy.transform.position = enemyFightPosition.position;
        Enemy.transform.rotation = enemyFightPosition.rotation;

        Enemy2.transform.position = enemyFightPosition2.position;
        Enemy2.transform.rotation = enemyFightPosition2.rotation;

        enemyGuard1Controller.hasStartedFight = true;
        enemyGuard2Controller.hasStartedFight = true;
        

        playerView.enabled = false;
        fightView.enabled = true;

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
            cardUIScript.SetCardName(card);

            Button cardButton = cardUI.GetComponent<Button>();
            cardButtons.Add(cardButton);

            cardButton.onClick.AddListener(() => SelectCard(card));
        }
    }

    private void SelectCard(string card)
    {
        // Only allow selection if there's a target and enough energy, unless it's a "Heal" card
        if (cardsSelected < 3 && currentEnergy > 0)
        {
            if (card == "Heal Card" || currentTarget != null) // Allow healing even if no target is selected
            {
                selectedCards.Add(card);
                cardsSelected++;
                currentEnergy--;
                UpdateEnergyUI();
                discardPile.Add(card);
                drawnCards.Remove(card);

                ApplyCardEffect(card);
            }
            else
            {
                // Notify the player that no enemy is selected
                Debug.Log("No enemy selected for attack!");
            }
        }
    }

    private void SetTarget(GameObject target, Slider targetHealth)
    {
        if (target == Enemy && enemyGuard1Controller.isEnemyDefeated)
        {
            Debug.Log("Enemy 1 is already defeated and cannot be targeted.");
            return;  // Do nothing if the enemy is defeated
        }

        if (target == Enemy2 && enemyGuard2Controller.isEnemyDefeated)
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
        if (card == "Attack Card")
        {
            // Only attack if a valid target is selected
            if (currentTarget != null)
            {
                Attack(currentTarget, 10);  // 10 damage is just an example
            }
            else
            {
                Debug.Log("No target selected. Cannot attack.");
            }
        }
        else if (card == "Heal Card")
        {
            // Always heal the player
            Heal(Player, 10);
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
            Attack(Player, 8);  // Perform attack on player
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
        if (target == Enemy && !enemyGuard1Controller.isEnemyDefeated)
        {
            EnemyHealth.value -= damage;
            if (EnemyHealth.value <= 0)
            {
                enemyGuard1Controller.isEnemyDefeated = true;  // Mark enemy 1 as defeated
                FallOver(target);
                enemy1Button.interactable = false;  // Disable the attack button for this enemy
                currentTarget = null;  // Reset target
            }
        }
        else if (target == Enemy2 && !enemyGuard2Controller.isEnemyDefeated)
        {
            Enemy2Health.value -= damage;
            if (Enemy2Health.value <= 0)
            {
                enemyGuard2Controller.isEnemyDefeated = true;  // Mark enemy 2 as defeated
                FallOver(target);
                enemy2Button.interactable = false;  // Disable the attack button for this enemy
                currentTarget = null;  // Reset target
            }
        }
    }

    private void FallOver(GameObject target)
    {
        // Handle animation of the enemy falling over
        Animator animator = target.GetComponent<Animator>();
        animator.SetTrigger("Fall");
    }

    public void Heal(GameObject target, float amount)
    {
        if (target == Player)
        {
            PlayerHealth.value += amount;
            if (PlayerHealth.value > PlayerHealth.maxValue)
                PlayerHealth.value = PlayerHealth.maxValue;
        }
        else if (target == Enemy)
        {
            EnemyHealth.value += amount;
            if (EnemyHealth.value > EnemyHealth.maxValue)
                EnemyHealth.value = EnemyHealth.maxValue;
        }
        else if (target == Enemy2)
        {
            Enemy2Health.value += amount;
            if (Enemy2Health.value > Enemy2Health.maxValue)
                Enemy2Health.value = Enemy2Health.maxValue;
        }
    }

    public void UpdateEnergyUI()
    {
        energyText.text = "Energy: " + currentEnergy;
    }

    private void LogDeckAndDiscardState()
    {
        Debug.Log("Deck: " + string.Join(", ", deck));
        Debug.Log("Discard Pile: " + string.Join(", ", discardPile));
    }
}
