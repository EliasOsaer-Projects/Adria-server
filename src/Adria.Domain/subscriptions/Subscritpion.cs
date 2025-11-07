namespace Adria.Domain;

public class Subscritpion
{
    public Guid SubscriptionId { get; set; }
    public string Type { get; set; }
    public decimal PriceCycle { get; set; }
    public string Description { get; set; }
    public bool IsMostPopular { get; set; }

    public Subscritpion(Guid id, string type, decimal price, string description, bool isMostPopular)
    {
        SubscriptionId = id;
        Type = type;
        PriceCycle = price;
        Description = description;
        IsMostPopular = isMostPopular;
    }
}
