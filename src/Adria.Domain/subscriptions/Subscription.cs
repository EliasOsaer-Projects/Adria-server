namespace Adria.Domain;

public class Subscription
{
    public string Type { get; set; }
    public decimal PriceCycle { get; set; }
    public string Description { get; set; }

    public Subscription(string type, decimal price, string description)
    {
        Type = type;
        PriceCycle = price;
        Description = description;
    }



}
