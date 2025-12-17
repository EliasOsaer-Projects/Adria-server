using Taskly.Infrastructure.Persistence.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;
using System.Data.Common;

namespace Adria.Infrastructure;

public sealed class CheckValidSubscription
{
    private static readonly string DEACTIVATE_USERSUBSCRIPTION = @"Update UserSubscription
    SET Status = 'Inactive'
    Where Enddate < CURDATE()
    AND Status = 'Active'";

    private static readonly string MONTHLY_PAYMENT = @"UPDATE User u
    JOIN UserSubscription us ON u.AdriaID = us.AdriaID
    JOIN Subscription s ON s.SubscriptionID = us.SubscriptionID
    SET u.Wallet = u.Wallet - s.Price,
      us.EndDate = DATE_ADD(us.EndDate, INTERVAL 1 MONTH)
    WHERE us.Status = 'Active'
      AND u.Wallet >= s.Price
      AND us.EndDate <= CURDATE();";

    private readonly string _conn;

    public CheckValidSubscription(IConfiguration configuration)
    {
        _conn = configuration["Persistence:ConnectionString"]! ??
          throw new InvalidOperationException("Persistence:ConnectionString is missing from configuration.");
    }

    public async Task Check()
    {
        await using var conn = new MySqlConnection(_conn);
        await conn.OpenAsync();
        //All changes in here first
        //Think of this as a apart database of sorts
        await using var tran = await conn.BeginTransactionAsync();

        try
        {
            await using var command = conn.CreateCommand();
            command.Transaction = tran;
            command.CommandText = MONTHLY_PAYMENT;
            await command.ExecuteNonQueryAsync();

            await using var command2 = conn.CreateCommand();
            command2.Transaction = tran;
            command2.CommandText = DEACTIVATE_USERSUBSCRIPTION;
            await command2.ExecuteNonQueryAsync();

            //If everything succeed it is committed to the "real" database.
            await tran.CommitAsync();
        }
        catch (DbException ex)
        {
            //If one fails reset everything and no changes.
            await tran.RollbackAsync();
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
                // Check if day has passed since last run
                if (_lastCheck < DateTime.UtcNow.Date)
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

