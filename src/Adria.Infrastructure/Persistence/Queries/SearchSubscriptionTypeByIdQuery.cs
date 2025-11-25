using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Microsoft.Extensions.Logging;
using System.Data.Common;
namespace Adria.Infrastructure;

public sealed class SearchSubscriptionTypeByIdQuery : ISearchSubscriptionTypeByIdQuery
{
    private readonly string query = "select * from Subscription where SubscriptionID = @id";
    private readonly ILogger _logger;
    private readonly string _connectionString;
    private readonly DbProviderFactory _factory;

    public SearchSubscriptionTypeByIdQuery(DbProviderFactory factory, string connectionString, ILogger logger)
    {
        _factory = factory;
        _connectionString = connectionString;
        _logger = logger;
    }

    public async Task<SubscriptionData> Fetch(Guid id)
    {
        _logger.LogInformation("Fetching subscription data for id {Id}", id);

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
            if (!await reader.ReadAsync())
            {
                _logger.LogInformation("{Id} wasn't found", id);
                return null!;
            }

            var subId = reader.GetOrdinal("SubscriptionID");
            var type = reader.GetOrdinal("Type");
            var price = reader.GetOrdinal("Price");
            var mostpopular = reader.GetOrdinal("IsMostPopular");
            var description = reader.GetString(reader.GetOrdinal("Description"));

            _logger.LogInformation("Found subscription for Id: {SubscriptionId} of the type: {Type}", subscriptionId, type);
            return new(
                SubscriptionId: reader.GetGuid(subId),
                Name: reader.GetString(type),
                Price: reader.GetDecimal(price),
                Description: description.Split(',', StringSplitOptions.TrimEntries).ToList(),
                IsMostPopular: reader.GetBoolean(mostpopular)
            );
        }
    }
}
