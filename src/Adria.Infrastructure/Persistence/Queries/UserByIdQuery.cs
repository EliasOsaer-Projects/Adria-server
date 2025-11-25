using System.Data.Common;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Microsoft.Extensions.Logging;

namespace Taskly.Infrastructure.Persistence.Queries;

public sealed class UserByIdQuery : IUserByIdQuery
{
    private static readonly string QRY = @"
        SELECT 
            User.AdriaID, 
            User.Name, 
            User.Wallet, 
            User.PointsBalance, 
            User.Streak, 
            User.Occupation,
            cs.UserSubscriptionID, 
            cs.Status, 
            Subscription.SubscriptionID, 
            Subscription.Type AS SubscriptionType, 
            Subscription.Price, 
            Subscription.Description, 
            Subscription.IsMostPopular
        FROM `User`
        LEFT JOIN `UserSubscription` cs ON User.AdriaID = cs.AdriaID AND cs.Status = 'Active'
        LEFT JOIN `Subscription` ON cs.SubscriptionID = Subscription.SubscriptionID
        WHERE User.AdriaID = @Id;
";
    
    private readonly DbProviderFactory _factory;
    private readonly string _connectionString;
    private readonly ILogger<UserByIdQuery> _logger;

    public UserByIdQuery(
        DbProviderFactory factory,
        string connectionString,
        ILogger<UserByIdQuery> logger)
    {
        _factory = factory;
        _connectionString = connectionString;
        _logger = logger;
    }

    public async Task<UserData?> Fetch(Guid userId)
    {
        _logger.LogInformation("Starting to fetch user with ID {UserId}", userId);

        using var connection = _factory.CreateConnection()
            ?? throw new InvalidOperationException("DbProviderFactory returned a null DbConnection.");

        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = QRY;

        var parameter = command.CreateParameter();
        parameter.ParameterName = "@Id";
        parameter.Value = userId;
        command.Parameters.Add(parameter);

        using var reader = await command.ExecuteReaderAsync();


        if (!await reader.ReadAsync())
        {
            _logger.LogInformation("User with ID {UserId} not found", userId);
            return null;
        }
        
        var id = reader.GetGuid(reader.GetOrdinal("AdriaID"));
        var name = reader.GetString(reader.GetOrdinal("Name"));
        var wallet = reader.GetDecimal(reader.GetOrdinal("Wallet"));
        var points = reader.GetInt32(reader.GetOrdinal("PointsBalance"));
        var streak = reader.GetInt32(reader.GetOrdinal("Streak"));
        int ordOccupation = reader.GetOrdinal("Occupation");
        string occupation = await reader.IsDBNullAsync(ordOccupation) 
            ? "" 
            : reader.GetString(ordOccupation);
        
        Guid? subscriptionId = await reader.IsDBNullAsync(reader.GetOrdinal("SubscriptionID"))
            ? null
            : Guid.Parse(reader.GetString(reader.GetOrdinal("SubscriptionID")));

        SubscriptionData? subscription = null;
        if (subscriptionId.HasValue)
        {
            subscription = new SubscriptionData(
                reader.GetGuid(reader.GetOrdinal("SubscriptionID")),
                reader.GetString(reader.GetOrdinal("SubscriptionType")),
                reader.GetDecimal(reader.GetOrdinal("Price")),
                reader.GetString(reader.GetOrdinal("Description")).Split(',', StringSplitOptions.TrimEntries).ToList(),
                reader.GetBoolean(reader.GetOrdinal("IsMostPopular"))

            );
        }

        _logger.LogInformation("Found user with ID {UserId}", userId);

    return new UserData(
        id,
        name,
        wallet,
        points,
        streak,
        subscription,
        occupation
    );
}
}
