using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameControllerRoom2 : MonoBehaviour
{
    public Camera playerView;
    public Camera fightView;
    public GameObject FightUI;

    [SerializeField] private GameObject Player = null;
    [SerializeField] private GameObject Enemy = null;
    [SerializeField] private GameObject Enemy2 = null;

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
    public Transform playerFightPosition; // Predefined player position for the fight
    public Transform enemyFightPosition;  // Predefined enemy position for the fight
    public Transform enemyFightPosition2;

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

    public void StartFight()
    {
        // Hide the fight prompt and show the fight UI
        FightUI.SetActive(true);

        // Reset the player's and enemy's positions to predefined fight positions
        Player.transform.position = playerFightPosition.position;
        Player.transform.rotation = playerFightPosition.rotation;

        Enemy.transform.position = enemyFightPosition.position;
        Enemy.transform.rotation = enemyFightPosition.rotation;

        Enemy2.transform.position = enemyFightPosition2.position;
        Enemy2.transform.rotation = enemyFightPosition2.rotation;

        // Switch cameras to the fight view
        playerView.enabled = false;
        fightView.enabled = true;

        FightUI.SetActive(true); // Activate the fight UI in the GameController
        InitializeDeck();
        DrawCards(5);
    }

    public void InitializeDeck()
    {
        if (PlayerData.instance != null)
        {
            deck.AddRange(PlayerData.instance.cardInventory);
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
        if (cardsSelected < 3 && currentEnergy > 0)
        {
            selectedCards.Add(card);
            cardsSelected++;
            currentEnergy--;
            UpdateEnergyUI();
            discardPile.Add(card);
            drawnCards.Remove(card);

            ApplyCardEffect(card);
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
        if (card == "Attack")
        {
            Attack(Enemy, 10);
        }
        else if (card == "Heal")
        {
            Heal(Player, 10);
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
            Attack(Player, 20);
            Heal(Enemy, 5);
        }
        else
        {
            Attack(Player, 10);
            Heal(Enemy, 10);
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
                PlayerData.instance.SavePlayerHealth(PlayerHealth.value); // Save updated health
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
                PlayerData.instance.SavePlayerHealth(PlayerHealth.value); // Save updated health
            }
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
            Enemy.GetComponent<GhostEnemyController>().isEnemyDefeated = true;
        }
        else if (target == Player)
        {
            resultText.text = "You Lose!";
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
}
