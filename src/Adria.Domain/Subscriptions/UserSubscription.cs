namespace Adria.Domain;

//This is a value object for User.
public class UserSubscription
{
    public Subscription CurrentSubscription { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; }

    public UserSubscription(Subscription currentSubscription, DateTime date)
    {
        CurrentSubscription = currentSubscription;
        StartDate = date;
        EndDate = StartDate.Add(new TimeSpan(30, 0, 0, 0));

        Status = "Active";
        EnsureValidDate(this, date);
    }

    //Change current subscription and all data.
    public void ChangeSubscription(Subscription subscription, DateTime date)
    {
        CurrentSubscription = subscription;
        ChangeDates(date);
        Status = "Active";
    }

    //Changing dates.
    public void ChangeDates(DateTime date)
    {
        StartDate = date;
        EndDate = StartDate.Add(new TimeSpan(30, 0, 0, 0));
        EnsureValidDate(this, date);
    }

    //To check if subscription is still valid.
    public void SetPaid(bool paid)
    {
        Status = paid ? "Active" : "Inactive";
    }

    //Montly Payment
    public decimal PaySubscription(decimal wallet, DateTime date)
    {
        if (wallet >= CurrentSubscription.PriceCycle)
        {
            SetPaid(true);
            ChangeDates(date);
            return wallet - CurrentSubscription.PriceCycle;
        }

        SetPaid(false);
        return wallet;
    }

    //Security
    private static void EnsureValidDate(UserSubscription subscription, DateTime date)
    {
        if (date > subscription.EndDate)
            throw new GenesisException("new Date is younger than End date");
    }
    
    public void Cancel()
    {
        Status = "Inactive";
        EndDate = DateTime.UtcNow;
    }
}
