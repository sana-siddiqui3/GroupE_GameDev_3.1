using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
}
