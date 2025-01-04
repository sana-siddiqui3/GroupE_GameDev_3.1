using UnityEngine; // Add this directive for the Sprite type

[System.Serializable]
public class InventoryItem
{
    public string itemName;       // Name of the item
    public Sprite itemSprite;     // Sprite representing the item
    public string itemDescription; // Description for tooltips or lore
}