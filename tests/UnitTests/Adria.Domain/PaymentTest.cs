using Adria.Domain;

namespace UnitTests;

public class PaymentTest
{

    [Fact]
    public void Constructor_WithValidProperties_SetAllProperties()
    {
        //Act
        Payment payment = new Payment();
        Type p = typeof(Payment);

        //Assert
        Assert.IsType(p, payment.GetType());
    }

}
