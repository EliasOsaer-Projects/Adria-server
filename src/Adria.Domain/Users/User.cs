namespace Adria.Domain;

public class User
{
    public Guid AdriaId { get; set; }
    public string Name { get; set; }
    public bool Role { get; set; }
    public int PointsBalance { get; set; }
    public DateTime LastPayment { get; set; }
    public int Streak { get; set; }

    public User(Guid id, string name, bool role, DateTime lastPayment)
    {
        AdriaId = id;
        Name = name;
        Role = role;
        PointsBalance = 0;
        LastPayment = lastPayment;
        Streak = 0;
    }

}
