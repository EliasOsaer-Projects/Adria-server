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

        Status = false;
    }

    public void ChangeSubscripion(Subscritpion subscritpion, DateTime date)
    {
        Subscritpion = subscritpion;
        ChangeDates(date);
        Status = false;
    }

    public void ChangeDates(DateTime date)
    {
        EnsureValidDate(this, date);
        StartDate = date;
        EndDate = StartDate.Add(new TimeSpan(30, 0, 0, 0));
    }

    public void IsSubscriptionPaid(bool status)
    {
        Status = status;
    }

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

    private static void EnsureValidDate(CustomerSubscription subscription, DateTime date)
    {
        if (date > subscription.EndDate)
            throw new ArgumentException("new Date is ypunger than End date");
    }
}
