using Adria.Domain.Shared.Exceptions;

namespace Adria.Domain;

public class User
{
    public string AdriaId { get; set; }
    public string Name { get; set; }
    public int PointsBalance { get; set; }
    public DateTime LastPayment { get; set; }
    public int Streak { get; set; }
    public decimal UserWallet { get; set; }
    public CustomerSubscription? CurrentSubscription { get; set; }
    public IReadOnlyCollection<CustomerCourse> Courses => _courses.AsReadOnly();
    private readonly List<CustomerCourse> _courses = [];

    //TODO Still need to add Cosmetics which doesn't exist;


    public User(string id, string name, decimal wallet)
    {
        EnsureUserIdIsValid(id);

        AdriaId = id;
        Name = name;
        PointsBalance = 0;
        Streak = 0;
        UserWallet = wallet;
    }
    
    public User(string id, string name, decimal wallet, List<CustomerCourse> courses)
    {
        EnsureUserIdIsValid(id);

        AdriaId = id;
        Name = name;
        PointsBalance = 0;
        Streak = 0;
        UserWallet = wallet;
        _courses = courses;
    }
    
    
    public User(string id, string name,DateTime lastPayment , decimal wallet, Subscription subscription)
    {
        EnsureUserIdIsValid(id);

        AdriaId = id;
        Name = name;
        PointsBalance = 0;
        LastPayment = lastPayment;
        Streak = 0;
        UserWallet = wallet;

        CurrentSubscription = new(subscription, lastPayment);
    }
    
    public void addSubscription(DateTime lastPayment, Subscription subscription)
    {
        lastPayment = lastPayment.Date;
        CurrentSubscription = new CustomerSubscription(subscription, lastPayment);
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
        UserWallet += payment;
    }



    public void ChangeLastPaymentDate(DateTime lastPayment)
    {
        LastPayment = lastPayment;
    }

    public void PaySubscription()
    {
        UserWallet -= CurrentSubscription.PaySubscription(UserWallet, DateTime.Today);
    }

    public void ChangeSubscription(Subscription subscription)
    {

        LastPayment = DateTime.Today;

        PaySubscription();

        CurrentSubscription.ChangeSubscription(subscription, LastPayment);
    }

    public void EndSubscription()
    {
        CurrentSubscription.IsSubscriptionPaid(false);
    }

    public void EnlargeStreak()
    {
        Streak++;
    }

    public void ResetStreak()
    {
        Streak = 0;
    }

    public void AddCourse(CustomerCourse course)
    {
        _courses.Add(course);
    }

    public void RemoveCourse(Guid id)
    {
        DoesIdExist(this, id);

        _courses.RemoveAll(course => course.Id.Equals(id));
    }

    //Security
    private static void EnsureUserIdIsValid(string id)
    {
        if (id.Length <= 0)
            throw new ArgumentException("This user id is empty");
    }

    private static void DoesIdExist(User user, Guid id)
    {
        if (!user._courses.Any(course => course.Id.Equals(id)))
            throw new ElementNotFoundException($"This id called {id} doesn't exist in the database");
    }

    private static void EnsureEnoughPoints(User user, int points)
    {
        if (points > user.PointsBalance)
            throw new ArgumentException("User is too poor");
    }

}
