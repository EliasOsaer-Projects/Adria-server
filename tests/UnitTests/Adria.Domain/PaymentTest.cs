using Adria.Domain;

namespace UnitTests;

public class PaymentTest
{

    private readonly Guid _adriaId;
    private readonly string _name;
    private readonly decimal _wallet;
    private readonly User _user;
    private readonly Payment _payment;

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

        _payment = new Payment();
    }

    [Fact]
    public void Constructor_WithValidProperties_SetAllProperties()
    {
        //Act
        Type p = typeof(Payment);

        //Assert
        Assert.IsType(p, _payment);
        Assert.NotNull(_payment.TransactionID);
    }

    [Fact]
    public void SuccesfullPayment()
    {
        //Act
        _payment.Transaction(100, _user);

        Assert.Equal(900m, _user.Wallet);

    }

}
