using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Application.Subscriptions;
using Microsoft.Extensions.Logging;
using System.Data.Common;
namespace Adria.Infrastructure;

public class SearchSubscriptionTypeByIdQuery : ISearchSubscriptionTypeByIdQuery
{
    private readonly string query = "select Type, PricePerCycle from Subscription where SubscriptionID = '@id'";
    private readonly ILogger _logger;
    private readonly string _connectionString;
    private readonly DbProviderFactory _factory;

    public SearchSubscriptionTypeByIdQuery(DbProviderFactory factory, string connectionString, ILogger logger)
    {
        _factory = factory;
        _connectionString = connectionString;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<SubscriptionData>> Fetch(Guid id)
    {
        using var connection = _factory.CreateConnection() ?? throw new InvalidOperationException("DBFactory provided a null connection");

        connection.ConnectionString = _connectionString;

        await connection.OpenAsync();

        using var command = connection.CreateCommand();

        command.CommandText = query;

        var subscriptionId = command.CreateParameter();
        subscriptionId.ParameterName = "@id";
        subscriptionId.Value = id.ToString();

        command.Parameters.Add(subscriptionId);


        using (var reader = await command.ExecuteReaderAsync())
        {
            IDictionary<Guid, SubscriptionData> subscriptionTypes = new Dictionary<Guid, SubscriptionData>();
            while (await reader.ReadAsync())
            {
                var type = reader.GetOrdinal("Type");
                var price = reader.GetOrdinal("PricePerCycle");

                subscriptionTypes.Add(id, new(Type: reader.GetString(type), PriceCycle: reader.GetDecimal(price), "no Description"));
            }
            return subscriptionTypes.Values.ToList().AsReadOnly();
        }




    }
}
