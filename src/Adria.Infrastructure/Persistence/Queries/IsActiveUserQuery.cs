using System.Data.Common;
using Adria.Application.Contracts;
using Adria.Domain;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Queries;

public class IsActiveUserQuery
    : IIsActiveUserQuery
{
    private readonly DbProviderFactory _factory;
    private readonly string _connectionString;
    private readonly ILogger<IsActiveUserQuery> _logger;
    
    private static readonly string QRY = @"
        SELECT us.Status
        FROM UserSubscription us
        WHERE us.AdriaID = @UserId
    ";

    public IsActiveUserQuery(
        ILogger<IsActiveUserQuery> logger, 
        DbProviderFactory factory,
        string connectionString
    )
    {
        _factory = factory;
        _logger = logger;
        _connectionString = connectionString;
    }
    
    public async Task<bool> Fetch(Guid userId)
    {
        _logger.LogInformation("Fetching IsActiveUserQuery");
        
        using var connection = _factory.CreateConnection()
                               ?? throw new Exception("connection is null");
        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();
        
        using var command = connection.CreateCommand();
        command.CommandText = QRY;
        
        var parameter = command.CreateParameter();
        parameter.ParameterName = "@UserId";
        parameter.Value = userId;
        command.Parameters.Add(parameter);
        
        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            var status = reader.GetString(reader.GetOrdinal("Status"));
            _logger.LogInformation("User {UserId} has subscription status: {Status}", userId, status);
            return status.Equals("Active", StringComparison.OrdinalIgnoreCase);
        }
        
        throw new GenesisException("No active subscription found for user.");
    }
}