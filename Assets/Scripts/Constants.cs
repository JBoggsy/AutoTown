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

public static class Direction
{
    public static readonly Vector3Int North = new Vector3Int(0, 1, 0);
    public static readonly Vector3Int South = new Vector3Int(0, -1, 0);
    public static readonly Vector3Int East = new Vector3Int(1, 0, 0);
    public static readonly Vector3Int West = new Vector3Int(-1, 0, 0);
}
