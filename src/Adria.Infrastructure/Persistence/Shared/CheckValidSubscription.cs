using Taskly.Infrastructure.Persistence.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;
using System.Data.Common;

namespace Adria.Infrastructure;

public sealed class CheckValidSubscription
{
    private static readonly string _query = @"Update UserSubscription
    SET Status = 'Inactive'
    Where Enddate < CURDATE()
    AND Status = 'Active'";

    private readonly IConfiguration _configuration;
    private readonly string _conn;

    public CheckValidSubscription(IConfiguration configuration)
    {
        _configuration = configuration;
        _conn = _configuration["Persistence:ConnectionString"]! ??
          throw new InvalidOperationException("Persistence:ConnectionString is missing from configuration.");
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
    private readonly ILogger<SubscriptionCheckService> _logger;

    // Tracks when the last check happened
    private DateTime _lastCheck = DateTime.MinValue;


    public SubscriptionCheckService(CheckValidSubscription checker, ILogger<SubscriptionCheckService> logger)
    {
        _checker = checker;
        _logger = logger;

    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Check if 30 days have passed since last run
                if (_lastCheck < DateTime.Today)
                {
                    await _checker.Check(); // running our query.
                    _lastCheck = DateTime.UtcNow;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error occurd during subscription check");
            }

            // Sleep for a shorter interval, e.g., 1 day
            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }
}

