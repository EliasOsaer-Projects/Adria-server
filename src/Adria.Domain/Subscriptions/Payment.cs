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

    public decimal Transaction(decimal amount, decimal wallet)
    {
        EnsureValidPayment(amount, wallet);
        TransactionDate = DateTime.Today;

        wallet -= amount;

        return wallet;
    }

    private void EnsureValidPayment(decimal amount, decimal wallet)
    {
        if (amount > wallet)
            throw new ArgumentException("The user is too poor to pay");
    }
}
