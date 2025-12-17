using System.Data.Common;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.Shared.Helpers;
using Microsoft.Extensions.Logging;
using Taskly.Infrastructure.Persistence.Shared;

namespace Adria.Infrastructure.Persistence.Queries;

public class GetUserCosmeticsQuery
    : IGetUserCosmeticsQuery
{
    private static readonly string QRY = @"
        SELECT ci.CosmeticItemID, ci.ItemName, ci.ItemType, ci.CosmeticImage, ci.PointsCost
        FROM UserCosmeticItem uc
        JOIN CosmeticItem ci ON uc.CosmeticItemID = ci.CosmeticItemID
        WHERE uc.AdriaID = @UserId;
    ";

    private readonly string GET_COSMETIC_FOR_USER = @"
        SELECT 
            c.CosmeticItemID, 
            c.ItemName, 
            c.ItemType, 
            c.PointsCost
        FROM CosmeticItem c
        WHERE c.ItemName = 'UltraMax'
          AND EXISTS (
              SELECT 1 
              FROM UserSubscription us
              JOIN Subscription s ON us.SubscriptionID = s.SubscriptionID
              WHERE us.AdriaID = @UserId
                AND s.Type = 'Ultra'
                AND us.Status = 'Active'
                AND NOW() BETWEEN us.StartDate AND us.EndDate
          );
    ";
    
    private readonly DbProviderFactory _factory;
    private readonly string _connectionString;
    private readonly ILogger<GetUserCosmeticsQuery> _logger;
    private readonly List<CosmeticItemData> _cosmetics;
    
    public GetUserCosmeticsQuery(
        DbProviderFactory factory,
        string connectionString,
        ILogger<GetUserCosmeticsQuery> logger
    )
    {
        _factory = factory;
        _connectionString = connectionString;
        _logger = logger;
        _cosmetics = new List<CosmeticItemData>();
    }
    
    public async Task GetUserCosmetics(Guid userId)
    {
        using var connection = _factory.CreateConnection()
                               ?? throw new AdriaDatabaseException("connection is null");
        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = QRY;
        
        var parameter = command.CreateParameter();
        parameter.ParameterName = "@UserId";
        parameter.Value = userId.ToString();
        command.Parameters.Add(parameter);
        
        await using var reader = await command.ExecuteReaderAsync();
        
        if (!reader.HasRows)
        {
            _logger.LogInformation("No cosmetics found for user with ID {UserId}", userId);
        }
        
        while (await reader.ReadAsync())
        {
            _cosmetics.Add(new CosmeticItemData(
                cosmeticId: reader.GetGuid(reader.GetOrdinal("CosmeticItemID")),
                itemName: reader.GetString(reader.GetOrdinal("ItemName")),
                itemType: reader.GetString(reader.GetOrdinal("ItemType")),
                points: reader.GetInt32(reader.GetOrdinal("PointsCost")),
                image: await Helper.ReadBlobAsync(reader, reader.GetOrdinal("CosmeticImage"))
            ));
        }
    }

    public async Task CheckForUltraMaxCosmetic(Guid userId)
    {
        using var connection = _factory.CreateConnection()
                               ?? throw new AdriaDatabaseException("connection is null");
        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = GET_COSMETIC_FOR_USER;
        
        var parameter = command.CreateParameter();
        parameter.ParameterName = "@UserId";
        parameter.Value = userId.ToString();
        command.Parameters.Add(parameter);
        
        await using var reader = await command.ExecuteReaderAsync();
        
        if (!reader.HasRows)
        {
            _logger.LogInformation("User is not eligible for UltraMax cosmetic with ID {UserId}", userId);
            return;
        }
        
        while (await reader.ReadAsync())
        {
            _cosmetics.Add(new CosmeticItemData(
                cosmeticId: reader.GetGuid(reader.GetOrdinal("CosmeticItemID")),
                itemName: reader.GetString(reader.GetOrdinal("ItemName")),
                itemType: reader.GetString(reader.GetOrdinal("ItemType")),
                points: reader.GetInt32(reader.GetOrdinal("PointsCost")),
                image: await Helper.ReadBlobAsync(reader, reader.GetOrdinal("CosmeticImage"))
            ));
        }
    }
    
    public async Task<IReadOnlyCollection<CosmeticItemData>> Fetch(Guid userId)
    {
        _logger.LogInformation("Fetching cosmetics for user with ID {UserId}", userId);
        await GetUserCosmetics(userId);
        await CheckForUltraMaxCosmetic(userId);
        return _cosmetics.AsReadOnly();
    }
}