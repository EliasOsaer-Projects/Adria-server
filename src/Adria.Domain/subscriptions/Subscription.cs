namespace Adria.Domain;

public class Subscription
{
    public string Type { get; set; }
    public decimal PriceCycle { get; set; }

    public Subscription(string type, decimal price)
    {
        Type = type;
        PriceCycle = price;
    }
}