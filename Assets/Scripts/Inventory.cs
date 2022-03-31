﻿using System;
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

    public bool ExtractItem(ItemType itemType)
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

    public bool IsEmpty() { return AmountHeld == 0; }
    public bool IsFull() { return AmountHeld == Capacity; }
    public bool ContainsItem(ItemType type) { return ItemAmounts.ContainsKey(type); }
}

public interface IInventory
{
    public bool InsertItemIntoInventory(ItemType itemType);
    public bool ExtractItemFromInventory(ItemType itemType);
    public bool InventoryFull();
    public bool InventoryEmpty();
    public bool InventoryContainsItem(ItemType itemType);
    public bool InventoryAllowsItem(ItemType itemType);
}