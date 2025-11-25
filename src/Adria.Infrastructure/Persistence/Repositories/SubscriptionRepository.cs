using System.Data.Common;
using Adria.Domain;
using Adria.Domain.Subscriptions;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Repositories;

public sealed class SubscriptionRepository : ISubscriptionRepository
{
    private readonly DbProviderFactory _factory;
    private readonly string _connectionString;
    private readonly ILogger<SubscriptionRepository> _logger;

    public SubscriptionRepository(DbProviderFactory factory, string connectionString, ILogger<SubscriptionRepository> logger)
    {
        _factory = factory;
        _connectionString = connectionString;
        _logger = logger;
    }

    public async Task<Subscription?> GetById(Guid subscriptionId)
    {
        const string query = @"
            SELECT SubscriptionID, Type, Price, Description, IsMostPopular
            FROM Subscription
            WHERE SubscriptionID = @SubscriptionId;
        ";

        _logger.LogInformation("retrieving subscription {SubscriptionId}", subscriptionId);
        using var connection = _factory.CreateConnection()!;
        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();

        using var cmd = connection.CreateCommand();
        cmd.CommandText = query;
        cmd.Parameters.Add(CreateParameter(cmd, "@SubscriptionId", subscriptionId));

        using var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync()) return null;

        return new Subscription(
            reader.GetGuid(reader.GetOrdinal("SubscriptionID")),
            reader.GetString(reader.GetOrdinal("Type")),
            reader.GetDecimal(reader.GetOrdinal("Price")),
            reader.GetString(reader.GetOrdinal("Description")),
            reader.GetBoolean(reader.GetOrdinal("IsMostPopular"))
        );
    }

    private static DbParameter CreateParameter(DbCommand cmd, string name, object value)
    {
        var param = cmd.CreateParameter();
        param.ParameterName = name;
        param.Value = value;
        return param;
    }
}