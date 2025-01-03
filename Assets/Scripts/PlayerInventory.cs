using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public GameObject inventoryMenu;          // The inventory menu UI
    public Transform inventoryGrid;           // The grid layout container
    public GameObject inventorySlotPrefab;    // Prefab for an inventory slot
    public Sprite defaultItemSprite;          // Default sprite for empty slots
    public int totalSlotCount = 36;           // Total number of slots in the inventory grid

    private bool isInventoryOpen = false;

    private void Start()
    {
        inventoryMenu.SetActive(false); // Ensure the inventory menu is initially hidden
    }

    private void Update()
    {
        // Toggle inventory menu visibility with Tab
        if (UnityEngine.InputSystem.Keyboard.current.tabKey.wasPressedThisFrame)
        {
            isInventoryOpen = !isInventoryOpen;
            inventoryMenu.SetActive(isInventoryOpen);

            if (isInventoryOpen)
            {
                UpdateInventoryDisplay();
            }
        }
    }

    public void UpdateInventoryDisplay()
    {
        if (PlayerData.instance == null)
        {
            Debug.LogError("PlayerData.instance is still null!");
            return;
        }

        // Clear existing slots
        foreach (Transform child in inventoryGrid)
        {
            Destroy(child.gameObject);
        }

        // Populate the grid with items and empty slots
        for (int i = 0; i < totalSlotCount; i++)
        {
            GameObject slot = Instantiate(inventorySlotPrefab, inventoryGrid);

            // Update the ItemIcon child
            Transform itemIconTransform = slot.transform.Find("ItemIcon");
            if (itemIconTransform != null)
            {
                Image itemIconImage = itemIconTransform.GetComponent<Image>();
                if (itemIconImage != null)
                {
                    if (i < PlayerData.instance.inventory.Count)
                    {
                        // Slot has an item: Assign its sprite
                        InventoryItem item = PlayerData.instance.inventory[i];
                        itemIconImage.sprite = item.itemSprite != null ? item.itemSprite : defaultItemSprite;
                        itemIconImage.enabled = true; // Ensure the icon is visible
                    }
                    else
                    {
                        // Slot is empty: Hide the item icon
                        itemIconImage.sprite = null;
                        itemIconImage.enabled = false; // Disable the icon renderer
                    }
                }
                else
                {
                    Debug.LogWarning("ItemIcon is missing an Image component!");
                }
            }
            else
            {
                Debug.LogWarning("ItemIcon child not found in InventorySlot prefab!");
            }
        }
    }

    private Sprite GetItemSprite(string itemName)
    {
        // Logic to retrieve item-specific sprites
        // Replace this with your actual sprite-mapping logic
        // Example: Using a dictionary to map item names to sprites
        return Resources.Load<Sprite>($"{itemName}");
    }
}
