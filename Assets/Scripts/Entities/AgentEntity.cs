using UnityEngine;

public abstract class AgentEntity: WorldEntity, IMotileEntity, IInventoryEntity
{
    public AgentControlInterface AgentController { get; set; }

    public abstract Percept Perceive(RegionModel region);
    public Action DecideNextAction(RegionModel region)
    {
        return AgentController.DecideNextAction(Perceive(region));
    }

    public abstract bool Move(Vector3Int direction);
    public abstract bool Translate(Vector3Int translation);

    public abstract bool ExtractItemFromInventory(ItemType itemType);
    public abstract bool InsertItemIntoInventory(ItemType itemType);
    public abstract bool InventoryAllowsItem(ItemType itemType);
    public abstract bool InventoryContainsItem(ItemType itemType);
    public abstract bool InventoryEmpty();
    public abstract bool InventoryFull();
}