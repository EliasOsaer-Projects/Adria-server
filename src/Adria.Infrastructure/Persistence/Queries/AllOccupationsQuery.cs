using System.Data.Common;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Queries;


public class AllOccupationsQuery : IAllOccupationsQuery
{
    private readonly DbProviderFactory _factory;
    private readonly string _connectionString;
    private readonly ILogger<AllOccupationsQuery> _logger;

    private static readonly string QRY = @"
        SELECT OccupationName
        FROM Occupation;
    ";

    public AllOccupationsQuery(
        DbProviderFactory factory,
        string connectionString,
        ILogger<AllOccupationsQuery> logger)
    {
        _factory = factory;
        _connectionString = connectionString;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<OccupationData>> Fetch()
    {
        _logger.LogInformation("Starting to fetch all possible Occupations");
        
        using var connection = _factory.CreateConnection()!;
        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = QRY;

        using var reader = await command.ExecuteReaderAsync();

        var result = new List<OccupationData>();

        while (await reader.ReadAsync())
        {
            var occupation = reader.GetString(reader.GetOrdinal("OccupationName"));
            result.Add(new OccupationData(occupation));
        }
        
        _logger.LogInformation("Fetched {Count} occupations", result.Count);

        return result.AsReadOnly();
    }
}