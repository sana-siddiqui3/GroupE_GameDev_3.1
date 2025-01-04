using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public InventoryItem item; // The item assigned to this slot
    private InventoryTooltip tooltip;

    private void Start()
    {
        tooltip = InventoryTooltip.instance;

        if (tooltip == null)
        {
            Debug.LogError("Tooltip instance not found! Ensure InventoryTooltip is present in the scene.");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null && tooltip != null)
        {
            tooltip.ShowTooltip(item.itemDescription);
            tooltip.UpdateTooltipPosition(Input.mousePosition);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltip != null)
        {
            tooltip.HideTooltip();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (item != null)
        {
            if (item.itemName == "Health Potion") // Check if the clicked item is a health potion
            {
                Debug.Log("Health potion clicked!");
                UseHealthPotion();
            }
            else if (item.itemName == "Purity Potion")
            {
                Debug.Log("Purity potion clicked!");
                UsePurityPotion();
            }
            else if (item.itemName == "Poison Potion")
            {
                Debug.Log("Poison potion clicked!");
                GameControllerRoom3 gameController = FindFirstObjectByType<GameControllerRoom3>();
                if (gameController != null)
                {
                    gameController.UsePoisonPotion();  // Call the UsePoisonPotion method to win the battle
                }
            }
        }
    }

    private void UseHealthPotion()
    {
        if (PlayerData.instance != null)
        {
            PlayerData.instance.HealPlayer(20); // Heal the player by 20
            Debug.Log("Used health potion!");

            PlayerData.instance.RemoveItem(item.itemName);
            InventoryTooltip.instance.gameObject.SetActive(false);

            PlayerInventory playerInventory = FindFirstObjectByType<PlayerInventory>();
            if (playerInventory != null)
            {
                playerInventory.UpdateInventoryDisplay();
            }
        }
    }

    private void UsePurityPotion()
    {
        if (PlayerData.instance != null)
        {
            HeartCrystal heartCrystal = FindFirstObjectByType<HeartCrystal>();
            if (heartCrystal != null && heartCrystal.IsPlayerNear())
            {
                PlayerData.instance.RemoveItem("Purity Potion");
                PlayerData.instance.AddItem("Purified Heart", Resources.Load<Sprite>("PurifiedHeart"), "A heart purified by the potion.");

                Debug.Log("Purified heart added to inventory.");

                InventoryTooltip.instance.gameObject.SetActive(false);
                PlayerInventory playerInventory = FindFirstObjectByType<PlayerInventory>();
                if (playerInventory != null)
                {
                    playerInventory.UpdateInventoryDisplay();
                }
            }
            else
            {
                Debug.LogWarning("You need to be near the Heart Crystal to use the Purity Potion.");
            }
        }
    }

}
