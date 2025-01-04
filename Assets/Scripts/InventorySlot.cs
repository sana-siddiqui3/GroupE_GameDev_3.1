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

            if (item.itemName == "Purity Potion")
            {
                Debug.Log("Purity potion clicked!");
                UsePurityPotion();
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
            PlayerInventory playerInventory = FindFirstObjectByType<PlayerInventory>();
            if (playerInventory != null)
            {
                playerInventory.UpdateInventoryDisplay();
            }
        }
    }

    private void UsePurityPotion()
    {
        // Check if PlayerData instance is valid
        if (PlayerData.instance != null)
        {
           
            // Find the HeartCrystal in the scene
            HeartCrystal heartCrystal = FindObjectOfType<HeartCrystal>();

            // Check if the player is near the Heart Crystal
            if (heartCrystal != null && heartCrystal.IsPlayerNear())
            {
                // Remove the Purity Potion from the inventory
                PlayerData.instance.RemoveItem("Purity Potion");

                // Add the Purified Heart to the inventory
                PlayerData.instance.AddItem("Purified Heart", Resources.Load<Sprite>("PurifiedHeart"), "A heart purified by the potion.");

                // Optionally update UI or show a message to the player
                Debug.Log("Purified heart added to inventory.");

                // Optionally update the inventory display
                InventoryTooltip.instance.gameObject.SetActive(false);
                PlayerInventory playerInventory = FindObjectOfType<PlayerInventory>();
                if (playerInventory != null)
                {
                    playerInventory.UpdateInventoryDisplay();
                }
            }
            else
            {
                // Notify the player they need to be near the Heart Crystal
                Debug.LogWarning("You need to be near the Heart Crystal to use the Purity Potion.");
            }
        }
    }
}
