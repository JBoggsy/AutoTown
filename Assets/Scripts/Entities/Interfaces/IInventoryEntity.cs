public interface IInventoryEntity
{
    public bool InsertItemIntoInventory(ItemType itemType);
    public bool ExtractItemFromInventory(ItemType itemType);
    public bool InventoryFull();
    public bool InventoryEmpty();
    public bool InventoryContainsItem(ItemType itemType);
    public bool InventoryAllowsItem(ItemType itemType);
}