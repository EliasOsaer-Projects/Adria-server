namespace Adria.Domain;

public class CosmeticItem
{
    public Guid CosmeticItemID { get; set; }
    public string ItemName { get; set; }
    public string ItemType { get; set; }
    public decimal Points { get; set; }

    public CosmeticItem(Guid cosmeticId, string itemName, string itemType, decimal points)
    {
        CosmeticItemID = cosmeticId;
        ItemName = itemName;
        ItemType = itemType;
        Points = points;
    }
}
