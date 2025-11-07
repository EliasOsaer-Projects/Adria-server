using System.Data.Common;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Queries;

public sealed class TopUsersByPointsQuery : ITopUsersByPointsQuery
{
    private static readonly string QRY = @"
        SELECT Name, PointsBalance
        FROM `User`
        ORDER BY PointsBalance DESC
        LIMIT 10;
    ";

    private readonly DbProviderFactory _factory;
    private readonly string _connectionString;
    private readonly ILogger<TopUsersByPointsQuery> _logger;

    public TopUsersByPointsQuery(
        DbProviderFactory factory,
        string connectionString,
        ILogger<TopUsersByPointsQuery> logger)
    {
        _factory = factory;
        _connectionString = connectionString;
        _logger = logger;
    }

    public async Task<IEnumerable<LeaderBoardData>> Fetch()
    {
        var users = new List<LeaderBoardData>();
        _logger.LogInformation("Fetching top 10 users by PointsBalance");

        using var connection = _factory.CreateConnection()
            ?? throw new InvalidOperationException("DbProviderFactory returned null connection.");

        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = QRY;

        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            users.Add(new LeaderBoardData(
                Name: reader.GetString(reader.GetOrdinal("Name")),
                PointsBalance: reader.GetInt32(reader.GetOrdinal("PointsBalance"))
            ));
        }

        return users;
    }
}
