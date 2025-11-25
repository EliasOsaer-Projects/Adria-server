namespace Adria.Domain;

public class Payment
{
    public Guid TransactionID { get; set; }
    public decimal AdriaCoinAmount { get; set; }
    public DateTime TransactionDate { get; set; }

    public Payment(decimal amount, User user)
    {
        TransactionID = Guid.NewGuid();

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
