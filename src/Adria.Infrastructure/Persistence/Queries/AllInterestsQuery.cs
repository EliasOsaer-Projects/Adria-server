using System.Data.Common;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Queries;


public class AllInterestsQuery : IAllInterestsQuery
{
    private readonly DbProviderFactory _factory;
    private readonly string _connectionString;
    private readonly ILogger<AllInterestsQuery> _logger;

    private static readonly string QRY = @"
        SELECT InterestName
        FROM Interest;
    ";

    public AllInterestsQuery(
        DbProviderFactory factory,
        string connectionString,
        ILogger<AllInterestsQuery> logger)
    {
        _factory = factory;
        _connectionString = connectionString;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<InterestData>> Fetch()
    {
        _logger.LogInformation("Starting to fetch all possible Interests");
        
        using var connection = _factory.CreateConnection()!;
        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = QRY;

        using var reader = await command.ExecuteReaderAsync();

        var result = new List<InterestData>();

        while (await reader.ReadAsync())
        {
            var interest = reader.GetString(reader.GetOrdinal("InterestName"));
            result.Add(new InterestData(interest));
        }
        
        _logger.LogInformation("Fetched {Count} interests", result.Count);

        return result.AsReadOnly();
    }
}