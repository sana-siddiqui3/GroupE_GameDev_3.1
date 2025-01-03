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
        }
    }

    private void UseHealthPotion()
    {
        if (PlayerData.instance != null)
        {
            PlayerData.instance.HealPlayer(20); // Heal the player by 20
            Debug.Log("Used health potion!");

            // Optionally remove the health potion from the inventory
            PlayerData.instance.RemoveItem(item.itemName);

            // Hide the tooltip
            InventoryTooltip.instance.gameObject.SetActive(false);

            // Update the inventory display
            PlayerInventory playerInventory = FindObjectOfType<PlayerInventory>();
            if (playerInventory != null)
            {
                playerInventory.UpdateInventoryDisplay();
            }
        }
    }
}
