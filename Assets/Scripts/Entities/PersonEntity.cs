using System;
using System.Collections.Generic;
using UnityEngine;

public class PersonEntity : IWorldEntity, IInventory
{
    public PersonMonobehaviour Monobehaviour { protected get; set; }

    public Vector3Int Position { get; protected set; }

    public string FirstName { get; protected set; }
    public string LastName { get; protected set; }
    public int Health { get; protected set; }
    public int Hunger { get; protected set; }
    public int Water { get; protected set; }
    public int Energy { get; protected set; }
    protected Inventory Inventory { get; set; }


    public PersonEntity(int x, int y)
    {
        Position = new Vector3Int(x, y, 0);
        Inventory = new Inventory(20);
    }


    // ACTION METHODS
    /////////////////

    protected bool Move(Vector3Int movement)
    {

        Position += movement;
        Monobehaviour.SetNeedsUpdate();
        return true;
    }

    protected bool StoreItemInBuilding(int buildingID, ItemType item) { throw new NotImplementedException(); }
    protected bool DropItem(ItemType itemType) { throw new NotImplementedException(); }
    protected bool MineRock(int rockID) { throw new NotImplementedException(); }
    protected bool ChopTree(int treeID) { throw new NotImplementedException(); }

    public void Simulate ()
    {
        if (Position.x == 8)
        {
            if (Position.y < 12)
            {
                Move(Direction.North);
            } else
            {
                Move(Direction.East);
            }
        } else if (Position.x == 9)
        {
            if (Position.y > 8)
            {
                Move(Direction.South);
            } else
            {
                Move(Direction.West);
            }
        }
    }

    public bool InsertItemIntoInventory(ItemType itemType) { return Inventory.AddItem(itemType); }

    public bool ExtractItemFromInventory(ItemType itemType) { return Inventory.ExtractItem(itemType); }

    public bool InventoryFull() { return Inventory.IsFull(); }

    public bool InventoryEmpty() { return Inventory.IsEmpty(); }

    public bool InventoryContainsItem(ItemType itemType) { return Inventory.ContainsItem(itemType); }

    public bool InventoryAllowsItem(ItemType itemType) { return true; }
}