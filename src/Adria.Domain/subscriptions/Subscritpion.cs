namespace Adria.Domain;

public class Subscritpion
{
    public string Type { get; set; }
    public decimal PriceCycle { get; set; }
    public string Description { get; set; }

    public Subscritpion(string type, decimal price, string description)
    {
        Type = type;
        PriceCycle = price;
        Description = description;
    }



}
