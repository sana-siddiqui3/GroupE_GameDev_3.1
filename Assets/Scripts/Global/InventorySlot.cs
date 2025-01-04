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

                // Try to find GameControllerRoom4, GameControllerRoom3, or GameControllerRoom2
                GameControllerRoom4 gameControllerRoom4 = FindFirstObjectByType<GameControllerRoom4>();
                GameControllerRoom3 gameControllerRoom3 = FindFirstObjectByType<GameControllerRoom3>();
                GameControllerRoom2 gameControllerRoom2 = FindFirstObjectByType<GameControllerRoom2>();

                // If in Room 4, use the poison potion in Room 4
                if (gameControllerRoom4 != null)
                {
                    gameControllerRoom4.UsePoisonPotion();  // Skip the battle by using the poison potion in Room 4
                }
                // If in Room 3, use the poison potion in Room 3
                else if (gameControllerRoom3 != null)
                {
                    gameControllerRoom3.UsePoisonPotion();  // Skip the battle by using the poison potion in Room 3
                }
                // If in Room 2, use the poison potion in Room 2
                else if (gameControllerRoom2 != null)
                {
                    gameControllerRoom2.UsePoisonPotion();  // Skip the battle by using the poison potion in Room 2
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
