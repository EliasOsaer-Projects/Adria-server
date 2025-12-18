using System.Data.Common;
using System.Threading.Tasks;
using Adria.Domain;
using Adria.Domain.Subscriptions;
using Adria.Domain.Users;
using Adria.Domain.Shared.Exceptions;
using Adria.Infrastructure.Persistence.Shared;
using Microsoft.Extensions.Logging;
using Taskly.Infrastructure.Persistence.Shared;

namespace Adria.Infrastructure.Persistence.Repositories;

public sealed class AdoUserSubscriptionRepository : AbstractAdoRepository, IUserSubscriptionRepository
{
    private readonly ILogger<AdoUserSubscriptionRepository> _logger;
    private readonly ISubscriptionRepository _subscriptionRepository;

    private static readonly string TABLE_USER_SUBS = "UserSubscription";
    private static readonly string TABLE_USER = "User";
    private static readonly string TABLE_SUB = "Subscription";
    private static readonly string COL_WALLET = "Wallet";
    private static readonly string COL_PRICE = "Price";
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

    private static readonly string PAY_SUBSCRIPTION = $@"
      UPDATE {TABLE_USER} u
      JOIN {TABLE_SUB} s ON s.{COL_SUBS_ID} = @SubsId
      SET u.{COL_WALLET} = u.{COL_WALLET} - s.{COL_PRICE}
      WHERE u.{COL_USER_ID} = @UserId
      AND u.{COL_WALLET} >= s.{COL_PRICE};
      ";
    
    private static readonly string PAY_SUBSCRIPTION_WITH_PROMO = $@"
    UPDATE {TABLE_USER} u
    JOIN {TABLE_SUB} s ON s.{COL_SUBS_ID} = @SubsId
    JOIN PromoCode p ON p.Code = @Code
    SET u.{COL_WALLET} = u.{COL_WALLET} - (s.{COL_PRICE} * (1 - (p.Discount / 100)))
    WHERE u.{COL_USER_ID} = @UserId
      AND p.Active = TRUE
      AND u.{COL_WALLET} >= (s.{COL_PRICE} * (1 - (p.Discount / 100)));
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

    public async Task Payment(Guid userId, Guid subId)
    {
        var param = CreateParameter("@UserId", userId.ToString());//NOSONAR
        var param2 = CreateParameter("@SubsId", subId.ToString());
        await ExecuteNonQueryAsync(PAY_SUBSCRIPTION, new[] { param, param2 });
    }
    
    public async Task PaymentWithPromo(Guid userId, Guid subId, string promoCode)
    {
        var parameters = new[]
        {
            CreateParameter("@UserId", userId.ToString()),//NOSONAR
            CreateParameter("@SubsId", subId.ToString()),
            CreateParameter("@Code", promoCode)
        };

        await ExecuteNonQueryAsync(PAY_SUBSCRIPTION_WITH_PROMO, parameters);
    }

    public async Task<UserSubscription?> ById(Guid userId)
    {
        var param = CreateParameter("@UserId", userId.ToString());//NOSONAR
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
                userSubscription.SetPaid(false);
            }

            return userSubscription;
        }
        finally
        {
            await reader.DisposeAsync();
        }
    }

    public async Task Save(User user, UserSubscription subscription, string? promoCode)
    {
        var exists = await ById(user.AdriaId);
        var query = exists == null ? INSERT_SUBSCRIPTION : UPDATE_SUBSCRIPTION;

        if (promoCode is null)
        {
            await Payment(user.AdriaId, subscription.CurrentSubscription.SubscriptionId);
        }
        else
        {
            await PaymentWithPromo(
                user.AdriaId,
                subscription.CurrentSubscription.SubscriptionId,
                promoCode
            );
        }
        
        try
        {
            var parameters = new[]
            {
                CreateParameter("@Id", Guid.NewGuid().ToString()), // for insert only
                CreateParameter("@UserId", user.AdriaId.ToString()),//NOSONAR
                CreateParameter("@SubsId", subscription.CurrentSubscription.SubscriptionId),
                CreateParameter("@Start", subscription.StartDate),
                CreateParameter("@End", subscription.EndDate),
                CreateParameter("@Status", subscription.Status)
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
