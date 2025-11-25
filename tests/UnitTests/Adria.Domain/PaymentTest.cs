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
        Assert.IsType(p, _payment.GetType());
        Assert.False(null == _payment.TransactionID);
    }

    public void SuccesfullPayment()
    {
        //Act
        _user.Wallet = _payment.Transaction(100, _user.Wallet);

        Assert.Equal(900m, _user.Wallet);

    }

}
