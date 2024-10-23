using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public Camera playerView;
    public Camera fightView;
    public GameObject FightUI;

    [SerializeField] private GameObject Player = null;
    [SerializeField] private GameObject Enemy = null;

    [SerializeField] private Slider PlayerHealth = null;
    [SerializeField] private Slider EnemyHealth = null;
    [SerializeField] private Button attackBtn = null;
    [SerializeField] private Button healBtn = null;
    [SerializeField] private TextMeshProUGUI resultText = null;  

    private bool isPlayerTurn = true;
    private bool isGameOver = false;  
    private EnemyTrigger enemyTrigger; 

    void Start()
    {
        FightUI.SetActive(false);  // Ensure FightUI is hidden at the start
        enemyTrigger = FindFirstObjectByType<EnemyTrigger>(); 
    }

    // Applies damage to the target and checks for defeat
    private void Attack(GameObject target, float damage)
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
            if (PlayerHealth.value <= 0)
            {
                PlayerHealth.value = 0;  
                FallOver(target);
            }
        }

        changeTurn();  
    }

    // Heals the target by a specified amount
    private void Heal(GameObject target, float amount)
    {
        if (target == Enemy)
        {
            EnemyHealth.value += amount;
        }
        else
        {
            PlayerHealth.value += amount;
        }

        changeTurn();  
    }

    // Called when the player clicks the attack button
    public void BtnAttack()
    {
        Attack(Enemy, 10);
    }

    // Called when the player clicks the heal button
    public void BtnHeal()
    {
        Heal(Player, 5);
    }

    // Switches between player and enemy turns
    private void changeTurn()
    {
        if (isGameOver)
            return;

        isPlayerTurn = !isPlayerTurn;

        if (!isPlayerTurn)
        {
            attackBtn.interactable = false;
            healBtn.interactable = false;

            StartCoroutine(EnemyTurn());  
        }
        else
        {
            attackBtn.interactable = true;
            healBtn.interactable = true;
        }
    }

    // Handles the enemy's turn actions
    private IEnumerator EnemyTurn()
    {
        if (isGameOver)
        {
            yield break;  // Stop enemy turn if the game is over
        }

        yield return new WaitForSeconds(3); 

        int random = Random.Range(1, 3);

        if (random == 1)
        {
            Attack(Player, 12);  
        }
        else
        {
            Heal(Enemy, 3);  
        }
    }

    // Handles the outcome when a character is defeated
    private void FallOver(GameObject target)
    {
        target.transform.Rotate(new Vector3(90f, 0f, 0f));  // Rotate the defeated character

        attackBtn.interactable = false;
        healBtn.interactable = false;

        isGameOver = true; 

        // Display win/lose message
        if (target == Enemy)
        {
            resultText.text = "You Win!";
            fightView.enabled = false;
            playerView.enabled = true;

            FightUI.SetActive(false);

            // Let EnemyTrigger know the enemy is defeated
            if (enemyTrigger != null)
            {
                enemyTrigger.isEnemyDefeated = true;
            }
        }
        else if (target == Player)
        {
            resultText.text = "You Lose!";
        }
    }
}
