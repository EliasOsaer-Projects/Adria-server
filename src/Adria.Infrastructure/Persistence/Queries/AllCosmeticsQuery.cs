using Adria.Application.Contracts;
using System.Data.Common;
using Microsoft.Extensions.Logging;
namespace Adria.Infrastructure;

public class AllCosmeticsQuery : IAllCosmeticsQuery
{
    private readonly ILogger _logger;
    private readonly DbProviderFactory _factory;
    private readonly string _connectionString;

}
