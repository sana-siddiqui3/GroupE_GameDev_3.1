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
    [SerializeField] private TextMeshProUGUI resultText = null;  // UI text to display result

    private bool isPlayerTurn = true;
    private bool isGameOver = false;  // Flag to stop the game after win/loss
    private EnemyTrigger enemyTrigger; // Reference to the EnemyTrigger

    void Start()
    {
        // Ensure FightUI is hidden at the start
        FightUI.SetActive(false);
        enemyTrigger = FindFirstObjectByType<EnemyTrigger>(); // Get the reference to EnemyTrigger
    }

    private void Attack(GameObject target, float damage)
    {
        if (target == Enemy)
        {
            EnemyHealth.value -= damage;
            if (EnemyHealth.value <= 0)
            {
                EnemyHealth.value = 0;  // Clamp to zero
                FallOver(target);
            }
        }
        else
        {
            PlayerHealth.value -= damage;
            if (PlayerHealth.value <= 0)
            {
                PlayerHealth.value = 0;  // Clamp to zero
                FallOver(target);
            }
        }

        changeTurn();
    }

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

    public void BtnAttack()
    {
        Attack(Enemy, 10);
    }

    public void BtnHeal()
    {
        Heal(Player, 5);
    }

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

    private void FallOver(GameObject target)
    {
        
        target.transform.Rotate(new Vector3(90f, 0f, 0f));

        
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
