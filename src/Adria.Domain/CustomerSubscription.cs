namespace Adria.Domain;

//This is a value object for User.
public class CustomerSubscription
{
    public Subscritpion Subscritpion { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool Status { get; set; }

    public CustomerSubscription(Subscritpion subscritpion, DateTime date)
    {
        EnsureValidDate(this, date);
        Subscritpion = subscritpion;
        StartDate = date;
        EndDate = StartDate.Add(new TimeSpan(30, 0, 0, 0));

        Status = true;
    }

    //Change current subscription and all data.
    public void ChangeSubscripion(Subscritpion subscritpion, DateTime date)
    {
        Subscritpion = subscritpion;
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
        if (price >= Subscritpion.PriceCycle)
        {
            IsSubscriptionPaid(true);
            ChangeDates(date);
            return price - Subscritpion.PriceCycle;
        }

        IsSubscriptionPaid(false);
        return price;
    }

    //Security
    private static void EnsureValidDate(CustomerSubscription subscription, DateTime date)
    {
        if (date > subscription.EndDate)
            throw new ArgumentException("new Date is ypunger than End date");
    }
}
