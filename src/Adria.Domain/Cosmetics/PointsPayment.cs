namespace Adria.Domain;

public class PointsPayment
{
    public Guid PointsTransactionID { get; set; }
    public Guid CosmeticItemID { get; set; }
    public Guid AdriaID { get; set; }
    public decimal PointsAmount { get; set; }
    public DateTime TransactionDate { get; set; }

    public PointsPayment(CosmeticItem cosmeticItem, User user)
    {
        PointsTransactionID = Guid.NewGuid();
        CosmeticItemID = cosmeticItem.CosmeticItemID;
        AdriaID = user.AdriaId;
        PointsAmount = cosmeticItem.Points;
        TransactionDate = DateTime.Today;
    }
}
