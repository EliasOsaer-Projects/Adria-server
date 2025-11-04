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
        Subscritpion = subscritpion;
        StartDate = date;
        EndDate = StartDate.Add(new TimeSpan(30, 0, 0, 0));

        Status = false;
    }

}
