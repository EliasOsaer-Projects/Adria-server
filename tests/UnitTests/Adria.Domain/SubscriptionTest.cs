using Adria.Domain;

namespace UnitTests.Adria.Domain;

public class SubscriptionTest
{
    [Fact]
    public void Constructor_WithValidParameters_SetsAllProperties()
    {
        // Arrange
        var id = Guid.NewGuid();
        var subscriptionType = "Basic";
        var pricePerCycle = 25.99m;
        var description = "Basic subscription for 1 year.";
        var isMostPopular = false;
        
        //Act
        var subscription = new Subscription(
            id, 
            subscriptionType, 
            pricePerCycle, 
            description, 
            isMostPopular
        );
        
        // Assert
        Assert.Equal(id, subscription.SubscriptionId);
        Assert.Equal(subscriptionType, subscription.Type);
        Assert.Equal(pricePerCycle, subscription.PriceCycle);
        Assert.Equal(description, subscription.Description);
        Assert.Equal(isMostPopular, subscription.IsMostPopular);
    }
}