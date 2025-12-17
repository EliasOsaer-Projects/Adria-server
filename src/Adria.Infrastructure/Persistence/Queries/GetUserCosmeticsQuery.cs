using System.Data.Common;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.Shared.Helpers;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Queries;

public class GetUserCosmeticsQuery
    : IGetUserCosmeticsQuery
{
    private static readonly string QRY = @"
        SELECT ci.CosmeticItemID, ci.ItemName, ci.ItemType, ci.CosmeticImage, ci.PointsCost
        FROM UserCosmeticItem uc
        JOIN CosmeticItem ci ON uc.CosmeticItemID = ci.CosmeticItemID
        WHERE uc.UserID = @UserId;
    ";
    
    private readonly DbProviderFactory _factory;
    private readonly string _connectionString;
    private readonly ILogger<GetUserCosmeticsQuery> _logger;
    
    public GetUserCosmeticsQuery(
        DbProviderFactory factory,
        string connectionString,
        ILogger<GetUserCosmeticsQuery> logger
    )
    {
        _factory = factory;
        _connectionString = connectionString;
        _logger = logger;
    }
    
    public async Task<IReadOnlyCollection<CosmeticItemData>> Fetch(Guid userId)
    {
        _logger.LogInformation("Fetching cosmetics for user with ID {UserId}", userId);
        
        using var connection = _factory.CreateConnection()
            ?? throw new NullReferenceException("connection is null");
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
            return Array.Empty<CosmeticItemData>();
        }
        
        List<CosmeticItemData> cosmetics = new();

        while (await reader.ReadAsync())
        {
            cosmetics.Add(new CosmeticItemData(
                cosmeticId: reader.GetGuid(reader.GetOrdinal("CosmeticItemID")),
                itemName: reader.GetString(reader.GetOrdinal("ItemName")),
                itemType: reader.GetString(reader.GetOrdinal("ItemType")),
                points: reader.GetInt32(reader.GetOrdinal("PointsCost")),
                image: await Helper.ReadBlobAsync(reader, reader.GetOrdinal("CosmeticImage"))
            ));
        }

        return cosmetics.AsReadOnly();
    }
}