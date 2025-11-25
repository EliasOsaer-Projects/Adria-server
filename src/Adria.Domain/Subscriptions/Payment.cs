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

    public void Transaction(decimal amount, decimal wallet)
    {
        TransactionDate = DateTime.Today;
    }

    private void EnsureValidPayment(decimal amount, decimal wallet)
    {
        if (amount > wallet)
            throw new ArgumentException("The user is too poor to pay");
    }
}
