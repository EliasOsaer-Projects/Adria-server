using Adria.Main.Modules.Persistence;
using Adria.Main.Modules.UseCases;
using Adria.Main.Modules.WebApi;
using Adria.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder
    .Services
        .AddPersistenceModule(configuration)
        .AddWebApiModule(configuration)
        .AddUseCases();

builder
    .Services
        .AddSingleton<CheckValidSubscription>();
builder
    .Services
        .AddHostedService<SubscriptionCheckService>();

await builder
    .Build()
    .UsePersistenceModule()
    .UseWebApiModule()
    .RunAsync();
