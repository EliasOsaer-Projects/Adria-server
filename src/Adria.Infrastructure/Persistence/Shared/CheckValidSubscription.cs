using Taskly.Infrastructure.Persistence.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;
using System.Data.Common;

namespace Adria.Infrastructure;

public sealed class CheckValidSubscription
{
    private static readonly string _query = @"Update UserSubscription
    SET Status = 'Inactive'
    Where Enddate = CURDATE() - INTERVAL 30 DAY";

    private readonly IConfiguration _configuration;
    private readonly string _conn;

    public CheckValidSubscription(IConfiguration configuration)
    {
        _configuration = configuration;
        _conn = _configuration["Persistence:ConnectionString"]!;
    }

    public async Task Check()
    {
        try
        {
            await using var conn = new MySqlConnection(_conn);
            await conn.OpenAsync();

            await using var command = conn.CreateCommand();
            command.CommandText = _query;
            await command.ExecuteNonQueryAsync();
        }
        catch (DbException ex)
        {
            throw new AdriaDatabaseException("We got a database error", ex);
        }
    }
}

public class SubscriptionCheckService : BackgroundService
{
    private readonly CheckValidSubscription _checker;

    public SubscriptionCheckService(CheckValidSubscription checker)
    {
        _checker = checker;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _checker.Check();
            await Task.Delay(TimeSpan.FromDays(30), stoppingToken);
        }
    }
}

