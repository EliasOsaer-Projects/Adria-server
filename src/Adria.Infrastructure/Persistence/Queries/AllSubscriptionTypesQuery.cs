using System.Data.Common;
using Microsoft.Extensions.Logging;
using Adria.Application.Contracts;

namespace Adria.Infrastructure;

public class AllSubscriptionTypesQuery//: IAllSubscriptionTypes
{
    static private readonly string query = @"select * from  Subscription";

    private readonly DbProviderFactory _factory;
    private readonly string _connectionString;
    private readonly ILogger<IAllSubscriptionTypes> _logger;

    AllSubscriptionTypesQuery(DbProviderFactory factory, string connectionString, ILogger<IAllSubscriptionTypes> logger)
    {
        _factory = factory;
        _connectionString = connectionString;
        _logger = logger;
    }



}
