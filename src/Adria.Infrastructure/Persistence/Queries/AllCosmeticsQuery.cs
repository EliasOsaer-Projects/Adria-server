using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using System.Data.Common;
using Adria.Domain.Shared.Helpers;
using Microsoft.Extensions.Logging;
namespace Adria.Infrastructure;

public class AllCosmeticsQuery : IAllCosmeticsQuery
{
    private readonly ILogger<AllCosmeticsQuery> _logger;
    private readonly DbProviderFactory _factory;
    private readonly string _connectionString;

    private readonly string query = @"
    SELECT ci.CosmeticItemID, ci.ItemName, ci.ItemType, ci.PointsCost, ci.CosmeticImage
    FROM CosmeticItem ci
    WHERE NOT EXISTS (
        SELECT 1
        FROM UserCosmeticItem uc
        WHERE uc.AdriaID = @UserId
          AND uc.CosmeticItemID = ci.CosmeticItemID
    );
";


    public AllCosmeticsQuery(DbProviderFactory factory, string connectionString, ILogger<AllCosmeticsQuery> logger)
    {
        _factory = factory;
        _connectionString = connectionString;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<CosmeticItemData>> Fetch(Guid userId)
    {
        _logger.LogInformation("Fetching all cosmetics available for user with id {UserId}", userId);
        
        using var connection = _factory.CreateConnection() ?? throw new InvalidOperationException("DbProviderFactory provided a null connection string");

        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();

        using (var command = connection.CreateCommand())
        {
            command.CommandText = query;
            
            var userParam = command.CreateParameter();
            userParam.ParameterName = "@UserId";
            userParam.Value = userId.ToString();
            command.Parameters.Add(userParam);
            
            await using var reader = await command.ExecuteReaderAsync();
            
            List<CosmeticItemData> data = new List<CosmeticItemData>();

            while (await reader.ReadAsync())
            {
                var id = reader.GetOrdinal("CosmeticItemId");
                var name = reader.GetOrdinal("ItemName");
                var type = reader.GetOrdinal("ItemType");
                var points = reader.GetOrdinal("PointsCost");
                var image = await Helper.ReadBlobAsync(reader, reader.GetOrdinal("CosmeticImage"));

                data.Add(new CosmeticItemData(
                    reader.GetGuid(id),
                    reader.GetString(name),
                    reader.GetString(type),
                    reader.GetInt32(points),
                    image
                    ));
            }
            return data.AsReadOnly();
        }
    }
}
