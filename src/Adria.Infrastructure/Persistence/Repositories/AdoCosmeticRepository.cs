using System.Data.Common;
using Adria.Domain;
using Adria.Infrastructure.Persistence.Shared;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Repositories;

public class AdoCosmeticRepository
    : AbstractAdoRepository, ICosmeticRepository
{
    private static readonly string GET_COSMETIC_BY_ID = @"
        SELECT CosmeticItemID, ItemName, ItemType, PointsCost
        FROM CosmeticItem
        WHERE CosmeticItemID = @CosmeticId;
    ";
    
    private static readonly string INSERT_USER_COSMETIC = @"
        INSERT INTO UserCosmeticItem (AdriaID, CosmeticItemID)
        VALUES (@UserId, @CosmeticId);
    ";
    
    private static readonly string UPDATE_USER_POINTS = @"
        UPDATE User
        SET PointsBalance = @PointsBalance
        WHERE AdriaID = @UserId;
    ";
    
    private readonly ILogger<AdoCosmeticRepository> _logger;

    public AdoCosmeticRepository(
        DbProviderFactory factory, 
        string connectionString,
        ILogger<AdoCosmeticRepository> logger
    ) : base(factory, connectionString)
    {
        _logger = logger;
    }

    public async Task<CosmeticItem> ById(Guid cosmeticId)
    {
        _logger.LogInformation("Fetching cosmetic item with id {CosmeticId}", cosmeticId);
        DbParameter cid = CreateParameter("@CosmeticId", cosmeticId.ToString());
        
        using (DbDataReader dbDataReader = await ExecuteReaderAsync(GET_COSMETIC_BY_ID, [cid]))
        {
            if (!await dbDataReader.ReadAsync())
            {
                _logger.LogWarning("No cosmetic found with id {CosmeticId}", cosmeticId);
                throw new GenesisException($"No cosmetic found with id {cosmeticId}");
            }

            try
            {
                var cosmetic = new CosmeticItem(
                    Guid.Parse(dbDataReader.GetString(dbDataReader.GetOrdinal("CosmeticItemID"))),
                    dbDataReader.GetString(dbDataReader.GetOrdinal("ItemName")),
                    dbDataReader.GetString(dbDataReader.GetOrdinal("ItemType")),
                    dbDataReader.GetInt32(dbDataReader.GetOrdinal("PointsCost"))
                );
                
                _logger.LogInformation("Successfully fetched cosmetic: {ItemName}", cosmetic.ItemName);
                return cosmetic;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error mapping cosmetic data for id {CosmeticId}", cosmeticId);
                throw new GenesisException($"Error retrieving cosmetic with id {cosmeticId}");
            }
        }
    }
    public async Task Save(CosmeticItem cosmeticItem, User user)
    {
        _logger.LogInformation("Saving cosmetic item {CosmeticId} for user {UserId}\n Cost={cost} XP={xp}", cosmeticItem.CosmeticItemID, user.AdriaId, cosmeticItem.Points, user.PointsBalance);
        
        if (user.PointsBalance < cosmeticItem.Points)
        {
            throw new GenesisException("Insufficient points to purchase this cosmetic item.");
        }
        
        DbParameter userIdParam = CreateParameter("@UserId", user.AdriaId.ToString());
        DbParameter cosmeticIdParam = CreateParameter("@CosmeticId", cosmeticItem.CosmeticItemID.ToString());
        
        await ExecuteNonQueryAsync(INSERT_USER_COSMETIC, [userIdParam, cosmeticIdParam]);
        
        _logger.LogInformation("Cosmetic item {CosmeticId} successfully saved for user {UserId}", cosmeticItem.CosmeticItemID, user.AdriaId);
        
        await ExecuteNonQueryAsync(UPDATE_USER_POINTS, [
            CreateParameter("@PointsBalance", user.PointsBalance - cosmeticItem.Points),
            CreateParameter("@UserId", user.AdriaId.ToString())
        ]);
        
        _logger.LogInformation("User {UserId} points updated to {PointsBalance}", user.AdriaId, user.PointsBalance - cosmeticItem.Points);
    }
}