using Taskly.Infrastructure.Persistence.Shared;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data.Common;

namespace Adria.Infrastructure;

public sealed class CheckValidSubscription
{
    private static readonly string _query = @"Update UserSubscription
    SET Status = 'Inactive'
    Where Enddate = CURDATE() - INTERVAL 30 DAY";

    private readonly IConfiguration _configuration;
    private readonly string _conn;

    public CheckValidSubscription(IConfiguration configuration)
    {
        _configuration = configuration;
        _conn = _configuration["Persistence:ConnectionString"]!;
    }



    public void Check()
    {
        try
        {
            using var conn = new MySqlConnection(_conn);
            conn.Open();

            var command = conn.CreateCommand();
            command.CommandText = _query;
            command.ExecuteNonQuery();
        }
        catch (DbException ex)
        {
            throw new AdriaDatabaseException("We got a database error", ex);
        }
    }
}
