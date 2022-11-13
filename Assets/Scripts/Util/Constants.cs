﻿using System.Collections.Generic;
using UnityEngine;

public enum TerrainType
{
    Dirt,
    Grass,
    Rock,
    Water_Shallow,
    Water_Deep,
    Ice
}

public enum ResourceDepositType
{
    Tree,
    Rock
}

/**
    * An item is defined as anything which can be held in an inventory, and anything which
    * can be held in an inventory is an item.
    */
public enum ItemType
{
    Wood,
    Stone
}

public enum BuildingType
{
    Town_Center
}

public static class Constants
{
    public static ItemType ItemFromDeposit(ResourceDepositType deposit)
    {
        return deposit switch
        {
            ResourceDepositType.Tree => ItemType.Wood,
            ResourceDepositType.Rock => ItemType.Stone,
            _ => default
        };
    }
}