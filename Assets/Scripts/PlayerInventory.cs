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
            } else {
                // Disable the tooltip when the inventory is closed
                InventoryTooltip.instance.gameObject.SetActive(false);
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
            InventorySlot inventorySlot = slot.GetComponent<InventorySlot>();

            // Update the ItemIcon child
            Transform itemIconTransform = slot.transform.Find("ItemIcon");
            if (itemIconTransform != null)
            {
                Image itemIconImage = itemIconTransform.GetComponent<Image>();
                if (itemIconImage != null)
                {
                    if (i < PlayerData.instance.inventory.Count)
                    {
                        // Assign the item to the slot
                        InventoryItem item = PlayerData.instance.inventory[i];
                        inventorySlot.item = item;

                        // Update the icon
                        itemIconImage.sprite = item.itemSprite != null ? item.itemSprite : defaultItemSprite;
                        itemIconImage.enabled = true;
                    }
                    else
                    {
                        // Slot is empty
                        inventorySlot.item = null;
                        itemIconImage.sprite = null;
                        itemIconImage.enabled = false;
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
