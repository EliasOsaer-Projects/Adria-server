namespace Adria.Domain;

//This is a value object for User.
public class CustomerSubscription
{
    public Subscription CurrentSubscription { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool Status { get; set; }

    public CustomerSubscription(Subscription currentSubscription, DateTime date)
    {
        EnsureValidDate(this, date);
        CurrentSubscription = currentSubscription;
        StartDate = date;
        EndDate = StartDate.Add(new TimeSpan(30, 0, 0, 0));

        Status = true;
    }

    //Change current subscription and all data.
    public void ChangeSubscription(Subscription subscription, DateTime date)
    {
        CurrentSubscription = subscription;
        ChangeDates(date);
        Status = true;
    }

    //Changing dates.
    public void ChangeDates(DateTime date)
    {
        EnsureValidDate(this, date);
        StartDate = date;
        EndDate = StartDate.Add(new TimeSpan(30, 0, 0, 0));
    }

    //To check if subscription is still valid.
    public void IsSubscriptionPaid(bool status)
    {
        Status = status;
    }

    //Montly Payment
    public decimal PaySubscription(decimal price, DateTime date)
    {
        if (price >= CurrentSubscription.PriceCycle)
        {
            IsSubscriptionPaid(true);
            ChangeDates(date);
            return price - CurrentSubscription.PriceCycle;
        }

        IsSubscriptionPaid(false);
        return price;
    }

    //Security
    private static void EnsureValidDate(CustomerSubscription subscription, DateTime date)
    {
        if (date > subscription.EndDate)
            throw new ArgumentException("new Date is younger than End date");
    }
}
