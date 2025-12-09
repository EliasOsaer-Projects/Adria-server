using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using System.Data.Common;
using Microsoft.Extensions.Logging;
namespace Adria.Infrastructure;

public class AllCosmeticsQuery : IAllCosmeticsQuery
{
    private readonly ILogger<IAllCosmeticsQuery> _logger;
    private readonly DbProviderFactory _factory;
    private readonly string _connectionString;

    private readonly string query = "select * from Cosmetics";

    public AllCosmeticsQuery(DbProviderFactory factory, string connectionString, ILogger<IAllCosmeticsQuery> logger)
    {
        _factory = factory;
        _connectionString = connectionString;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<CosmeticItemData>> Fetch()
    {
        using var connection = _factory.CreateConnection() ?? throw new InvalidOperationException("DbProviderFactory provided a null connection string");

        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();

        using (var command = connection.CreateCommand())
        {
            command.CommandText = query;

            await using var reader = await command.ExecuteReaderAsync();

            List<CosmeticItemData> data = new List<CosmeticItemData>();

            while (await reader.ReadAsync())
            {
            }

        }





    }

}
