using UnityEngine;
using TMPro;

public class InventoryTooltip : MonoBehaviour
{
    public static InventoryTooltip instance; // Singleton instance

    public TextMeshProUGUI descriptionText; // Tooltip text element
    private RectTransform tooltipTransform; // Tooltip RectTransform
    private Vector2 offset = new Vector2(15, -15); // Offset to position tooltip relative to the cursor
    private float fixedHeight = 30f; // Fixed height for the tooltip
    private float padding = 10f; // Padding inside the tooltip

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        tooltipTransform = GetComponent<RectTransform>();
        gameObject.SetActive(false); // Start hidden
    }

    public void ShowTooltip(string description)
    {
        if (descriptionText != null)
        {
            descriptionText.text = description; // Set the description text

            // Calculate the preferred width of the text with padding
            float preferredWidth = descriptionText.preferredWidth + padding * 2;

            // Update the tooltip's RectTransform
            tooltipTransform.sizeDelta = new Vector2(preferredWidth, fixedHeight);
        }
        else
        {
            Debug.LogError("Tooltip's descriptionText is not assigned!");
        }

        gameObject.SetActive(true); // Show the tooltip
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false); // Hide the tooltip
    }

    public void UpdateTooltipPosition(Vector3 cursorPosition)
    {
        // Calculate the tooltip's position with the offset
        Vector3 tooltipPosition = cursorPosition + new Vector3(offset.x, offset.y, 0);

        // Clamp position within screen bounds
        float tooltipWidth = tooltipTransform.rect.width;
        float tooltipHeight = tooltipTransform.rect.height;

        tooltipPosition.x = Mathf.Clamp(tooltipPosition.x, 0, Screen.width - tooltipWidth);
        tooltipPosition.y = Mathf.Clamp(tooltipPosition.y, 0, Screen.height - tooltipHeight);

        tooltipTransform.position = tooltipPosition;
    }
}
