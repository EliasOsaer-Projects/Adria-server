using Adria.Domain.Shared.Exceptions;

namespace Adria.Domain;

public class User
{
    public Guid AdriaId { get; set; }
    public string Name { get; set; }
    public int PointsBalance { get; set; }
    public DateTime LastPayment { get; set; }
    public int Streak { get; set; }
    public Wallet UserWallet { get; set; }
    public IReadOnlyCollection<Course> Courses => _courses.AsReadOnly();
    private readonly List<Course> _courses = [];

    //TODO Still need to add subscription which doesn't exist;
    //TODO Still need to add Cosmetics which doesn't exist;


    public User(Guid id, string name, DateTime lastPayment)
    {
        EnsureUserIdIsValid(id);

        AdriaId = id;
        Name = name;
        PointsBalance = 0;
        LastPayment = lastPayment;
        Streak = 0;
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

    public void ChangeLastPaymentDate(DateTime lastpayment)
    {
        LastPayment = lastpayment;
    }

    public void Enlargestreak()
    {
        Streak++;
    }

    public void ResetStreak()
    {
        Streak = 0;
    }

    public void AddCourse(Course course)
    {
        EnsureUserIdIsValid(course.Id);
        _courses.Add(course);
    }

    public void RemoveCourse(Guid id)
    {
        EnsureUserIdIsValid(id);

        _courses.RemoveAll(course => course.Id.Equals(id));
    }

    //Security
    private static void EnsureUserIdIsValid(Guid id)
    {
        if (id == Guid.Empty)
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
