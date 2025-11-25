using Adria.Domain;

namespace UnitTests;

public class PaymentTest
{

    private readonly Guid _adriaId;
    private readonly string _name;
    private readonly decimal _wallet;
    private readonly User _user;

    public PaymentTest()
    {
        // Arrange
        _adriaId = Guid.NewGuid();
        _name = "Matthias Blomme";
        _wallet = 1000m;

        // Act
        _user = new User(
            _adriaId,
            _name,
            _wallet
        );

    }

    [Fact]
    public void Constructor_WithValidProperties_SetAllProperties()
    {
        //Act
        Payment _payment = new Payment(100, _user);

        Type p = typeof(Payment);

        //Assert
        Assert.IsType(p, _payment);
        Assert.NotEqual(Guid.Empty, _payment.TransactionID);
        Assert.Equal(900m, _user.Wallet);

    }


}
