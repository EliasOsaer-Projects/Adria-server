using Adria.Application.Subscriptions;
using UnitTests.Mocks;

namespace UnitTests.Adria.Application.Subscriptions;

public class GetDiscountByPromoCodeTests
{
    [Fact]
    public async Task ReturnDiscount_When_PromoCodeExists()
    {
        var mock = new MockDiscountByPromoCodeQuery();
        mock.AddPromoCode("aw3Som3Cod3", 10);

        var mockLogger = new MockLogger<GetDiscountByPromoCode>();
        var useCase = new GetDiscountByPromoCode(mock, mockLogger);
        
        var result = await useCase.Execute(new GetDiscountByPromoCodeInput("aw3Som3Cod3"));

        Assert.Equal(10, result);
    }

    [Fact]
    public async Task ReturnZero_When_PromoCodeDoesNotExist()
    {
        var mock = new MockDiscountByPromoCodeQuery();

        var mockLogger = new MockLogger<GetDiscountByPromoCode>();
        var useCase = new GetDiscountByPromoCode(mock, mockLogger);

        var result = await useCase.Execute(new GetDiscountByPromoCodeInput("INVALID"));

        Assert.Equal(0, result);
    }
}
