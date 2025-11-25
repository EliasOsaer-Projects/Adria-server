using System.Data.Common;
using Adria.Domain;
using Adria.Domain.Users;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly DbProviderFactory _factory;
    private readonly string _connectionString;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(DbProviderFactory factory, string connectionString, ILogger<UserRepository> logger)
    {
        _factory = factory;
        _connectionString = connectionString;
        _logger = logger;
    }

    public async Task<User?> GetById(Guid adriaId)
    {
        const string query = @"SELECT AdriaID, Name, Wallet, PointsBalance, Streak FROM `User` WHERE AdriaID = @UserId;";

        using var connection = _factory.CreateConnection()!;
        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();

        using var cmd = connection.CreateCommand();
        cmd.CommandText = query;
        cmd.Parameters.Add(CreateParameter(cmd, "@UserId", adriaId));

        using var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync()) return null;

        return new User(
            reader.GetGuid(reader.GetOrdinal("AdriaID")),
            reader.GetString(reader.GetOrdinal("Name")),
            reader.GetDecimal(reader.GetOrdinal("Wallet"))
        );
    }

    public async Task Save(User user)
    {
        const string query = @"
            UPDATE `User`
            SET Name = @Name, Wallet = @Wallet, PointsBalance = @PointsBalance, Streak = @Streak
            WHERE AdriaID = @UserId;
        ";

        using var connection = _factory.CreateConnection()!;
        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();

        using var cmd = connection.CreateCommand();
        cmd.CommandText = query;

        cmd.Parameters.Add(CreateParameter(cmd, "@UserId", user.AdriaId));
        cmd.Parameters.Add(CreateParameter(cmd, "@Name", user.Name));
        cmd.Parameters.Add(CreateParameter(cmd, "@Wallet", user.Wallet));
        cmd.Parameters.Add(CreateParameter(cmd, "@PointsBalance", user.PointsBalance));
        cmd.Parameters.Add(CreateParameter(cmd, "@Streak", user.Streak));

        await cmd.ExecuteNonQueryAsync();
        _logger.LogInformation("Saved user {UserId}", user.AdriaId);
    }

    private static DbParameter CreateParameter(DbCommand cmd, string name, object value)
    {
        var param = cmd.CreateParameter();
        param.ParameterName = name;
        param.Value = value;
        return param;
    }
}