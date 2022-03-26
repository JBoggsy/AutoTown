using System;
using System.Collections.Generic;

public class Inventory
{
    public int Capacity { get; protected set; }
    public int AmountHeld { get; protected set; }
    
    protected Dictionary<ItemType, int> ItemAmounts { get; set; } /// Holds the number of items of each type in this inventory.

    public Inventory(int capacity)
    {
        Capacity = capacity;
        AmountHeld = 0;
        ItemAmounts = new Dictionary<ItemType, int>();
    }

    public bool AddItem(ItemType itemType)
    {
        if (AmountHeld == Capacity) { return false; }

        if (ItemAmounts.ContainsKey(itemType)) { ItemAmounts[itemType]++; }
                                          else { ItemAmounts.Add(itemType, 1); }
        AmountHeld++;
        return true;
    }

    public bool RemoveItem(ItemType itemType)
    {
        if (!ItemAmounts.ContainsKey(itemType)) { return false; }

        ItemAmounts[itemType]--;
        if (ItemAmounts[itemType] == 0)
        {
            ItemAmounts.Remove(itemType);
        }
        AmountHeld--;
        return true;
    }
}