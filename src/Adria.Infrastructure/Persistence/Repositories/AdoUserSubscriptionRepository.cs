using System.Data.Common;
using Adria.Domain;
using Adria.Domain.Subscriptions;
using Adria.Domain.Users;
using Adria.Infrastructure.Persistence.Shared;
using Microsoft.Extensions.Logging;
using Taskly.Infrastructure.Persistence.Shared;

namespace Adria.Infrastructure.Persistence.Repositories;

public sealed class AdoUserSubscriptionRepository : AbstractAdoRepository, IUserSubscriptionRepository
{
    private readonly ILogger<AdoUserSubscriptionRepository> _logger;
    private readonly ISubscriptionRepository _subscriptionRepository;

    private static readonly string TABLE_USER_SUBS = "UserSubscription";
    private static readonly string COL_ID = "UserSubscriptionID";
    private static readonly string COL_USER_ID = "AdriaID";
    private static readonly string COL_SUBS_ID = "SubscriptionID";
    private static readonly string COL_START = "StartDate";
    private static readonly string COL_END = "EndDate";
    private static readonly string COL_STATUS = "Status";

    private static readonly string SELECT_BY_USER_ID = $@"
        SELECT {COL_ID}, {COL_SUBS_ID}, {COL_START}, {COL_END}, {COL_STATUS}
        FROM {TABLE_USER_SUBS}
        WHERE {COL_USER_ID} = @UserId;
    ";

    private static readonly string INSERT_SUBSCRIPTION = $@"
        INSERT INTO {TABLE_USER_SUBS} ({COL_ID}, {COL_USER_ID}, {COL_SUBS_ID}, {COL_START}, {COL_END}, {COL_STATUS})
        VALUES (@Id, @UserId, @SubsId, @Start, @End, @Status);
    ";

    private static readonly string UPDATE_SUBSCRIPTION = $@"
        UPDATE {TABLE_USER_SUBS}
        SET {COL_SUBS_ID} = @SubsId, {COL_START} = @Start, {COL_END} = @End, {COL_STATUS} = @Status
        WHERE {COL_USER_ID} = @UserId;
    ";

    public AdoUserSubscriptionRepository(
        DbProviderFactory factory,
        string connectionString,
        ILogger<AdoUserSubscriptionRepository> logger,
        ISubscriptionRepository subscriptionRepository
    ) : base(factory, connectionString)
    {
        _logger = logger;
        _subscriptionRepository = subscriptionRepository;
    }

    public async Task<UserSubscription?> ById(Guid userId)
    {
        var param = CreateParameter("@UserId", userId.ToString());
        await using var reader = await ExecuteReaderAsync(SELECT_BY_USER_ID, new[] { param });

        if (!await reader.ReadAsync()) return null;

        try
        {
            var subsId = reader.GetGuid(reader.GetOrdinal(COL_SUBS_ID));
            var startDate = reader.GetDateTime(reader.GetOrdinal(COL_START));
            var statusStr = reader.GetString(reader.GetOrdinal(COL_STATUS));

            var subscription = await _subscriptionRepository.ById(subsId);
            if (subscription == null) return null;

            var userSubscription = new UserSubscription(subscription, startDate);
            if (!string.Equals(statusStr, "Active", StringComparison.OrdinalIgnoreCase))
            {
                userSubscription.IsSubscriptionPaid(false);
            }

            return userSubscription;
        }
        finally
        {
            await reader.DisposeAsync();
        }
    }

    public async Task Save(User user, UserSubscription userSubscription)
    {
        var exists = await ById(user.AdriaId);
        var query = exists == null ? INSERT_SUBSCRIPTION : UPDATE_SUBSCRIPTION;

        try
        {
            var parameters = new[]
            {
                CreateParameter("@Id", Guid.NewGuid().ToString()), // for insert only
                CreateParameter("@UserId", user.AdriaId.ToString()),
                CreateParameter("@SubsId", userSubscription.CurrentSubscription.SubscriptionId),
                CreateParameter("@Start", userSubscription.StartDate),
                CreateParameter("@End", userSubscription.EndDate),
                CreateParameter("@Status", userSubscription.Status ? "Active" : "Inactive")
            };

            await ExecuteNonQueryAsync(query, parameters);
            _logger.LogInformation("Saved subscription for user {UserId}", user.AdriaId);
        }
        catch (DbException ex)
        {
            _logger.LogError(ex, "Failed to save subscription for user {UserId}", user.AdriaId);
            throw new AdriaDatabaseException("Failed to save user subscription.", ex);
        }
    }
}
