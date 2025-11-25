namespace Adria.Domain;

public class Payment
{
    public Guid TransactionID { get; set; }
    public decimal AdriaCoinAmount { get; set; }
    public DateTime TransactionDate { get; set; }

    public Payment()
    {
        TransactionID = Guid.NewGuid();
    }


}
