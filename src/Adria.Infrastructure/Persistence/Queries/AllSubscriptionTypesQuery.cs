using System.Data.Common;
using Adria.Application;
using Microsoft.Extensions.Logging;
using Adria.Application;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;


namespace Adria.Infrastructure;

public sealed class AllSubscriptionTypesQuery : IAllSubscriptionTypes
{
    static private readonly string query = @"select Type, PricePerCycle from Subscription";

    private readonly DbProviderFactory _factory;
    private readonly string _connectionString;
    private readonly ILogger<IAllSubscriptionTypes> _logger;

    public AllSubscriptionTypesQuery(DbProviderFactory factory, string connectionString, ILogger<IAllSubscriptionTypes> logger)
    {
        _factory = factory;
        _connectionString = connectionString;
        _logger = logger;

    }

    public async Task<IReadOnlyCollection<SubscriptionData>> Fetch()
    {

        _logger.LogInformation("Fetching Subscription data ...");

        using var connection = _factory.CreateConnection() ?? throw new InvalidOperationException("DbProviderFactory provided a null connection string");

        connection.ConnectionString = _connectionString;

        await connection.OpenAsync();

        using (var command = connection.CreateCommand())
        {
            command.CommandText = query;

            using (var reader = command.ExecuteReader())
            {
                var subscriptions = new List<SubscriptionData>();
                while (await reader.ReadAsync())
                {
                    var name = reader.GetOrdinal("Type");
                    var price = reader.GetOrdinal("PricePerCycle");
                    var desc = "No description";
                    subscriptions.Add(new SubscriptionData(Type: reader.GetString(name), PriceCycle: reader.GetDecimal(price), desc));
                }
                return subscriptions;
            }

        }

    }

}
