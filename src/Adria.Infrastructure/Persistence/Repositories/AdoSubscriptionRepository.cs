using System.Data.Common;
using Adria.Domain;
using Adria.Domain.Subscriptions;
using Adria.Infrastructure.Persistence.Shared;
using Microsoft.Extensions.Logging;
using Taskly.Infrastructure.Persistence.Shared;

namespace Adria.Infrastructure.Persistence.Repositories;

public sealed class AdoSubscriptionRepository : AbstractAdoRepository, ISubscriptionRepository
{
    private readonly ILogger<AdoSubscriptionRepository> _logger;
    private static readonly string TABLE_SUBSCRIPTION = "Subscription";
    private static readonly string COL_ID = "SubscriptionID";
    private static readonly string COL_TYPE = "Type";
    private static readonly string COL_PRICE = "Price";
    private static readonly string COL_DESCRIPTION = "Description";
    private static readonly string COL_IS_MOST_POPULAR = "IsMostPopular";

    private static readonly string SELECT_SUBSCRIPTION_BY_ID = $@"
        SELECT {COL_ID}, {COL_TYPE}, {COL_PRICE}, {COL_DESCRIPTION}, {COL_IS_MOST_POPULAR}
        FROM {TABLE_SUBSCRIPTION}
        WHERE {COL_ID} = @SubscriptionId;
    ";

    public AdoSubscriptionRepository(
        DbProviderFactory factory,
        string connectionString,
        ILogger<AdoSubscriptionRepository> logger
    ) : base(factory, connectionString)
    {
        _logger = logger;
    }

    public async Task<Subscription?> ById(Guid subscriptionId)
    {
        DbParameter id = CreateParameter("@SubscriptionId", subscriptionId.ToString());
        DbDataReader dbDataReader = await ExecuteReaderAsync(SELECT_SUBSCRIPTION_BY_ID, [id]);
        try
        {
            if (await dbDataReader.ReadAsync())
            {
                return new Subscription(
                    dbDataReader.GetGuid(dbDataReader.GetOrdinal(COL_ID)),
                    dbDataReader.GetString(dbDataReader.GetOrdinal(COL_TYPE)),
                    dbDataReader.GetDecimal(dbDataReader.GetOrdinal(COL_PRICE)),
                    dbDataReader.GetString(dbDataReader.GetOrdinal(COL_DESCRIPTION)),
                    dbDataReader.GetBoolean(dbDataReader.GetOrdinal(COL_IS_MOST_POPULAR))
                );
            }

            return null;
        }
        catch (DbException ex)
        {
            _logger.LogError(ex, "Failed to read subscription with ID {SubscriptionId} from database.", subscriptionId);
            throw new AdriaDatabaseException("Failed to read subscription from database.", ex);
        }
        finally
        {
            _logger.LogInformation("Disposing DbDataReader for subscription with ID {SubscriptionId}.", subscriptionId);
            await dbDataReader.DisposeAsync();
        }
    }
}