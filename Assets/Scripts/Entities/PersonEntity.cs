using System;
using System.Collections.Generic;
using UnityEngine;

public class PersonEntity : AgentEntity
{
    public PersonMonobehaviour Monobehaviour { protected get; set; }

    public string FirstName { get; protected set; }
    public string LastName { get; protected set; }
    public int Hunger { get; protected set; }
    public int Water { get; protected set; }
    public int Energy { get; protected set; }
    protected Inventory Inventory { get; set; }

    public PersonEntity(int x, int y)
    {
        Position = new Vector3Int(x, y, 0);
        Inventory = new Inventory(20);
    }


    ////////////////////////
    // AI CONTROL METHODS //
    ////////////////////////
    
    public override Percept Perceive(RegionModel region)
    {
        Percept new_percept = new Percept(this);
        new_percept.Region = region;
        return new_percept;
    }

    public override bool ApplyNextAction(RegionModel region)
    {
        throw new NotImplementedException();
    }


    ////////////////////
    // ACTION METHODS //
    ////////////////////
    
    // Movement
    ///////////

    override public bool Move(Vector3Int direction)
    {

        Position += direction;
        Monobehaviour.SetNeedsUpdate();
        return true;
    }

    public override bool Translate(Vector3Int translation)
    {
        Position += translation;
        Monobehaviour.SetNeedsUpdate();
        return true;
    }

    // Gathering
    ////////////

    protected bool MineRock(int rockID) { throw new NotImplementedException(); }

    protected bool ChopTree(int treeID) { throw new NotImplementedException(); }

    // Inventory
    ////////////

    protected bool StoreItemInBuilding(int buildingID, ItemType item) { throw new NotImplementedException(); }

    protected bool DropItem(ItemType itemType) { throw new NotImplementedException(); }

    override public bool InsertItemIntoInventory(ItemType itemType) { return Inventory.AddItem(itemType); }

    override public bool ExtractItemFromInventory(ItemType itemType) { return Inventory.ExtractItem(itemType); }

    override public bool InventoryFull() { return Inventory.IsFull(); }

    override public bool InventoryEmpty() { return Inventory.IsEmpty(); }

    override public bool InventoryContainsItem(ItemType itemType) { return Inventory.ContainsItem(itemType); }

    override public bool InventoryAllowsItem(ItemType itemType) { return true; }



    ///////////////////////////////
    // ENTITY MANAGEMENT METHODS //
    ///////////////////////////////
    
    public override bool ApplyDamage(float damage) 
    {
        Health -= damage;
        return (Health <= 0);
    }

    public override bool ApplyHeal(float heal)
    {
        Health += heal;
        if (Health > MaxHealth)
        {
            Health = MaxHealth;
        }

        return (Health == MaxHealth);
    }

    public override void Destroy()
    {
        throw new NotImplementedException();
    }
}