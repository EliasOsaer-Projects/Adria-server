namespace Adria.Domain;

public class Wallet
{
    public string Name { get; set; }
    public decimal AdriaCoinAmount { get; set; }
    public decimal Income { get; set; }

    public Wallet(string name, decimal amount, decimal income)
    {
        EnsureValidAmount(amount);
        EnsureValidAmount(income);
        Name = name;
        AdriaCoinAmount = amount;
        Income = income;
    }

    public void AddMonthlyIncome()
    {
        AdriaCoinAmount += Income;
    }

    public void Transaction(decimal price)
    {
        EnsureValidAmount(price);

        AdriaCoinAmount -= price;
    }

    public void EnsureValidAmount(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentException("You aren't allowed to add a negative amount");
    }

}
