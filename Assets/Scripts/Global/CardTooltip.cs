using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardTooltip : MonoBehaviour
{
    public static CardTooltip instance; // Singleton instance

    public TextMeshProUGUI tooltipText; // Reference to the tooltip text
    private RectTransform tooltipTransform; // Tooltip's RectTransform
    public Vector2 offset = new Vector2(15, -15); // Offset for positioning the tooltip relative to the cursor

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
        gameObject.SetActive(false); // Initially hide the tooltip
    }

    public void ShowTooltip(string text, Vector3 position)
    {
        Debug.Log($"Showing tooltip: {text} at {position}");
        tooltipText.text = text; // Set the tooltip text
        tooltipTransform.position = position + new Vector3(offset.x, offset.y, 0); // Position the tooltip
        gameObject.SetActive(true); // Show the tooltip
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false); // Hide the tooltip
    }

    private void Update()
    {
        // Make the tooltip follow the cursor
        if (gameObject.activeSelf)
        {
            Vector3 mousePosition = Input.mousePosition;
            tooltipTransform.position = mousePosition + new Vector3(offset.x, offset.y, 0);
        }
    }
}
