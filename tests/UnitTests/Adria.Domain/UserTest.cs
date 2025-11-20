using Adria.Domain;

namespace UnitTests.Adria.Domain;

public class UserTest
{
    private readonly Guid _adriaId;
    private readonly string _name;
    private readonly decimal _wallet;
    private readonly User _user;
    
    public UserTest()
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
    public void Constructor_WithValidParameters_SetsAllProperties()
    {
        // Assert
        Assert.Equal(_adriaId, _user.AdriaId);
        Assert.Equal(_name, _user.Name);
        Assert.Equal(_wallet, _user.Wallet);
    }
    
    [Fact]
    public void AddPoints_AddsPointsToBalance()
    {
        // Act
        _user.addPoints(100);
        
        // Assert
        Assert.Equal(100, _user.PointsBalance);
    }

    [Fact]
    public void PayDay_IncreasesWalletAmount()
    {
        // Act
        _user.PayDay(500);
        
        // Assert
        Assert.Equal(1500, _user.Wallet);
    }
    
    [Fact]
    public void ResetStreak_SetsStreakToZero()
    {
        // Arrange
        _user.Streak = 5;
        
        // Act
        _user.ResetStreak();
    }
    
    [Fact]
    public void IncreaseStreak_IncreasesStreak()
    {
        // Arrange
        _user.Streak = 5;
        
        // Act
        _user.IncreaseStreak();
        
        // Assert
        Assert.Equal(6, _user.Streak);
    }
}