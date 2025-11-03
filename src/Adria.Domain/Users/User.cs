namespace Adria.Domain;

public class User
{
    public Guid AdriaId { get; set; }
    public string Name { get; set; }
    public int PointsBalance { get; set; }
    public DateTime LastPayment { get; set; }
    public int Streak { get; set; }

    public User(Guid id, string name, DateTime lastPayment)
    {
        EnsureUserIdIsValid(id);

        AdriaId = id;
        Name = name;
        PointsBalance = 0;
        LastPayment = lastPayment;
        Streak = 0;
    }



    private void EnsureUserIdIsValid(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("This user id is empty");
    }

}
