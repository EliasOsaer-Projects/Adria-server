namespace Adria.Domain;

public class Subscritpion
{
    public SubscritpionType Type { get; set; }
    public decimal PriceCycle { get; set; }
    public string Description { get; set; }

    public Subscritpion(SubscritpionType type, decimal price, string description)
    {
        Type = type;
        PriceCycle = price;
        Description = description;
    }



}
