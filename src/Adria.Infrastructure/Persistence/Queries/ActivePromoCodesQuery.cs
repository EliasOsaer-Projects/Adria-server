using System.Data.Common;
using Adria.Application.Contracts;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Queries;

public class CurrentPromoCodesQuery : IActivePromoCodesQuery
{
    private readonly DbProviderFactory _factory;
    private readonly string _connectionString;
    private readonly ILogger<CurrentPromoCodesQuery> _logger;

    private static readonly string QRY = @"
        SELECT Code
        FROM PromoCode
        WHERE Active = TRUE;
    ";

    public CurrentPromoCodesQuery(
        DbProviderFactory factory,
        string connectionString,
        ILogger<CurrentPromoCodesQuery> logger)
    {
        _factory = factory;
        _connectionString = connectionString;
        _logger = logger;
    }

    public async Task<IEnumerable<string>> Fetch()
    {
        _logger.LogInformation("Starting to fetch all active promo codes");
        
        using var conn = _factory.CreateConnection()!;
        conn.ConnectionString = _connectionString;
        await conn.OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = QRY;

        using var reader = await cmd.ExecuteReaderAsync();

        var result = new List<string>();

        while (await reader.ReadAsync())
        {
            var code = reader.GetString(reader.GetOrdinal("Code"));
            result.Add(code);
        }
        
        _logger.LogInformation("Fetched {Count} active promo codes", result.Count);

        return result.AsReadOnly();
    }
}