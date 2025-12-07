using Adria.Domain.Shared.Exceptions;

namespace Adria.Domain;

public class User
{
    public Guid AdriaId { get; set; }
    public string Name { get; set; }
    public decimal Wallet { get; set; }
    public int PointsBalance { get; set; }
    public int Streak { get; set; }
    
    public UserSubscription? CurrentSubscription { get; private set; }
    
    public string? Occupation { get; private set; }
    public IList<string>? Interests { get; private set; }


    public User(Guid id, string name, decimal wallet)
    {
        EnsureUserIdIsValid(id);

        AdriaId = id;
        Name = name;
        PointsBalance = 0;
        Streak = 0;
        Wallet = wallet;
    }

    public void addPoints(int points)
    {
        PointsBalance += points;
    }

    public void RemovePoints(int points)
    {
        EnsureEnoughPoints(this, points);
        PointsBalance -= points;
    }

    //Add Money in wallet
    public void PayDay(decimal payment)
    {
        Wallet += payment;
    }

    public void IncreaseStreak()
    {
        Streak++;
    }

    public void ResetStreak()
    {
        Streak = 0;
    }

    //Security
    private static void EnsureUserIdIsValid(Guid id)
    {
        if (id.ToString().Length <= 0)
            throw new GenesisException("This user id is empty");
    }

    private static void EnsureEnoughPoints(User user, int points)
    {
        if (points > user.PointsBalance)
            throw new GenesisException("User is too poor");
    }

    public void Subscribe(Subscription subscription)
    {
        if (CurrentSubscription == null)
            CurrentSubscription = new UserSubscription(subscription, DateTime.UtcNow);
        else
            CurrentSubscription.ChangeSubscription(subscription, DateTime.UtcNow);
    }

    public void AddOccupation(string occupation) //mogen er spaties zijn in een occupation?
    {
        if (string.IsNullOrWhiteSpace(occupation))
            throw new GenesisException("Occupation cannot be empty.");

        if (occupation.All(char.IsDigit))
            throw new GenesisException("Occupation cannot be a number.");
        
        Occupation = occupation.Trim();
    }

    public void AddInterests(List<String> interests)
    {
        if (interests.Count != 3)
            throw new GenesisException("A user must have exactly 3 interests.");
        
        Interests = interests;
    }
}
