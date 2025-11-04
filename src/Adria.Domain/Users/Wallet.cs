namespace Adria.Domain;

public class Wallet
{
    public string Name { get; set; }
    public decimal AdriaCoinAmount { get; set; }
    public decimal Income { get; set; }

    public Wallet(string name, decimal amount, decimal income)
    {
        Name = name;
        AdriaCoinAmount = amount;
        Income = income;
    }



}
