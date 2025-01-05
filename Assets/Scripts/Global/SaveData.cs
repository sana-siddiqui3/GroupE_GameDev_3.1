using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public float playerHealth;
    public int keysCollected;
    public string objective;
    public int difficulty;

    // We store item names instead of the full InventoryItem to keep JSON simple.
    public List<string> inventoryItemNames = new List<string>();
}
