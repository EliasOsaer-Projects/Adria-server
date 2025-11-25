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

    public void Transaction(decimal amount, User user)
    {
        EnsureValidPayment(amount, user.Wallet);
        TransactionDate = DateTime.Today;

        user.Wallet -= amount;
    }

    private void EnsureValidPayment(decimal amount, decimal wallet)
    {
        if (amount > wallet)
            throw new ArgumentException("The user is too poor to pay");
    }
}
