using System.Data.Common;
using Adria.Application.Contracts;
using Adria.Application.Subscriptions;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Queries;

public class DiscountByPromoCodeQuery : IDiscountByPromoCodeQuery
{
    private readonly DbProviderFactory _factory;
    private readonly string _connectionString;
    private readonly ILogger<DiscountByPromoCodeQuery> _logger;

    private static readonly string QRY = @"
        SELECT Discount
        FROM PromoCode
        WHERE Active = TRUE AND Code = @PromoCode;
    ";

    public DiscountByPromoCodeQuery(
        DbProviderFactory factory,
        string connectionString,
        ILogger<DiscountByPromoCodeQuery> logger)
    {
        _factory = factory;
        _connectionString = connectionString;
        _logger = logger;
    }

    public async Task<decimal> Fetch(string promoCode)
    {
        _logger.LogInformation("Starting to Check if the promocode {PromoCode} is correct and Fetching the discount",
            promoCode);

        using var connection = _factory.CreateConnection()!;
        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = QRY;

        var parameter = command.CreateParameter();
        parameter.ParameterName = "@PromoCode";
        parameter.Value = promoCode;
        command.Parameters.Add(parameter);

        using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
        {
            _logger.LogInformation("Discount for PromoCode {PromoCode} not found", promoCode);
            return new decimal(0.00);
        }
        
        var result = reader.GetDecimal(reader.GetOrdinal("Discount"));
        
        _logger.LogInformation("Discount for promocode {PromoCode} found", promoCode);
        return result;
    }
}