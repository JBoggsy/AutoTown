using UnityEngine;

public class TownCenterEntity : BuildingEntity, IInventoryEntity
{
    public TownCenterMonobehaviour Monobehaviour;

    public new readonly BuildingType buildingType = BuildingType.Town_Center;

    protected Inventory Inventory { get; set; }

    public TownCenterEntity(RegionModel region, int x, int y) : base(region)
    {
        Position = new Vector3Int(x, y, 0);
        Health = 100;
        Inventory = new Inventory(100);
    }

    public bool InsertItemIntoInventory(ItemType itemType) { return Inventory.AddItem(itemType); }

    public bool ExtractItemFromInventory(ItemType itemType) { return Inventory.ExtractItem(itemType); }

    public bool InventoryFull() { return Inventory.IsFull(); }

    public bool InventoryEmpty() { return Inventory.IsEmpty(); }

    public bool InventoryContainsItem(ItemType itemType) { return Inventory.ContainsItem(itemType); }

    public bool InventoryAllowsItem(ItemType itemType) { return true; }

    public override bool ApplyDamage(float damage)
    {
        throw new System.NotImplementedException();
    }

    public override bool ApplyHeal(float heal)
    {
        throw new System.NotImplementedException();
    }

    public override void Destroy()
    {
        throw new System.NotImplementedException();
    }
}