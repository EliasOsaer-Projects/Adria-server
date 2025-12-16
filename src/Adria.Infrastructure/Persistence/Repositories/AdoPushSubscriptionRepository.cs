using System.Data.Common;
using Adria.Domain.PushNotifications;
using Adria.Infrastructure.Persistence.Shared;
using Microsoft.Extensions.Logging;
using Taskly.Infrastructure.Persistence.Shared;

namespace Adria.Infrastructure.Persistence.Repositories;

public sealed class AdoPushSubscriptionRepository
    : AbstractAdoRepository, IPushSubscriptionRepository
{
    private readonly ILogger<AdoPushSubscriptionRepository> _logger;

    private const string TableName = "PushSubscriptions";

    private const string InsertSubscriptionSql = $@"
        INSERT INTO {TableName} (Endpoint, P256dh, Auth)
        VALUES (@endpoint, @p256dh, @auth)
        ON DUPLICATE KEY UPDATE
            P256dh = VALUES(P256dh),
            Auth = VALUES(Auth);";

    private const string SelectSubscriptionsSql = $@"
        SELECT Endpoint, P256dh, Auth
        FROM {TableName};
    ";

    public AdoPushSubscriptionRepository(
        DbProviderFactory factory,
        string connectionString,
        ILogger<AdoPushSubscriptionRepository> logger
    ) : base(factory, connectionString)
    {
        _logger = logger;
    }

    public async Task SaveSubscription(PushSubscription pushSubscription)
    {
        DbParameter[] parameters =
        [
            CreateParameter("@endpoint", pushSubscription.Endpoint),
            CreateParameter("@p256dh", pushSubscription.P256dh),
            CreateParameter("@auth", pushSubscription.Auth)
        ];

        await ExecuteNonQueryAsync(InsertSubscriptionSql, parameters);
    }

    public IReadOnlyList<PushSubscription> GetSubscriptions()
    {
        var subscriptions = new List<PushSubscription>();
        try
        {
            using DbConnection connection = _factory.CreateConnection()
                ?? throw new InvalidOperationException("Failed to create a database connection.");

            connection.ConnectionString = _connectionString;
            connection.Open();

            using DbCommand command = connection.CreateCommand();
            command.CommandText = SelectSubscriptionsSql;

            using DbDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string endpoint = reader.GetString(0);
                string p256dh = reader.GetString(1);
                string auth = reader.GetString(2);

                subscriptions.Add(new PushSubscription(endpoint, p256dh, auth));
            }
        }
        catch (DbException exception)
        {
            _logger.LogError(exception, "Failed to load push subscriptions from database.");
            throw new AdriaDatabaseException("Failed to read push subscriptions from database.", exception);
        }

        return subscriptions;
    }
}
