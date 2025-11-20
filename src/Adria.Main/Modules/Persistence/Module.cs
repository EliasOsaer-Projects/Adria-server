using Adria.Infrastructure;
using System.Data.Common;
using Adria.Application.Contracts;
using Adria.Infrastructure.Persistence.Queries;
using Taskly.Infrastructure.Persistence.Queries;

namespace Adria.Main.Modules.Persistence;

public static class PersistenceModule
{
    private static string _connectionString = string.Empty;
    public static IServiceCollection AddPersistenceModule(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        _connectionString = configuration["Persistence:ConnectionString"]!;
        return services
            .AddAdoServices(configuration)
            .AddRepositories()
            .AddQueries();
    }

    public static WebApplication UsePersistenceModule(this WebApplication app)
    {
        return app.ApplyMigrations();
    }

    private static IServiceCollection AddRepositories(
        this IServiceCollection services
    )
    {
        // Configure repositories here.
        return services;
    }

    private static IServiceCollection AddQueries(
        this IServiceCollection services
    )
    {
        // Configure queries here.

        return services
            .AddScoped<IUserByIdQuery, UserByIdQuery>(serviceProvider =>
            {
                return new UserByIdQuery(
                    serviceProvider.GetRequiredService<DbProviderFactory>(),
                    _connectionString,
                    serviceProvider.GetRequiredService<ILogger<UserByIdQuery>>()
                );
            })
            .AddScoped<IAllSubscriptionTypes, AllSubscriptionTypesQuery>(serviceProvider =>
            {
                return new AllSubscriptionTypesQuery(serviceProvider.GetRequiredService<DbProviderFactory>(),
                    _connectionString,
                    serviceProvider.GetRequiredService<ILogger<AllSubscriptionTypesQuery>>()
                );
            })
            .AddScoped<ISearchSubscriptionTypeByIdQuery, SearchSubscriptionTypeByIdQuery>(serviceProvider =>
            {
                return new SearchSubscriptionTypeByIdQuery(serviceProvider.GetRequiredService<DbProviderFactory>(),
                    _connectionString,
                    serviceProvider.GetRequiredService<ILogger<SearchSubscriptionTypeByIdQuery>>()
                );
            })
            .AddScoped<ITopUsersByPointsQuery, TopUsersByPointsQuery>(serviceProvider =>
            {
                return new TopUsersByPointsQuery(serviceProvider.GetRequiredService<DbProviderFactory>(),
                    _connectionString,
                    serviceProvider.GetRequiredService<ILogger<TopUsersByPointsQuery>>()
                );
            })
            .AddScoped<IAllCoursesQuery, AllCoursesQuery>(serviceProvider =>
            {
                return new AllCoursesQuery(serviceProvider.GetRequiredService<DbProviderFactory>(),
                    _connectionString,
                    serviceProvider.GetRequiredService<ILogger<AllCoursesQuery>>()
                );
            })
            .AddScoped<IUserCoursesByUserQuery, SearchUserCoursesByUserQuery>(serviceProvider =>
            {
                return new SearchUserCoursesByUserQuery(serviceProvider.GetRequiredService<DbProviderFactory>(),
                    _connectionString,
                    serviceProvider.GetRequiredService<ILogger<SearchUserCoursesByUserQuery>>()
                );
            })
            .AddScoped<ISearchCourseByIdQuery, SearchCourseByIdQuery>(serviceProvider =>
            {
                return new SearchCourseByIdQuery(serviceProvider.GetRequiredService<DbProviderFactory>(),
                    _connectionString,
                    serviceProvider.GetRequiredService<ILogger<SearchCourseByIdQuery>>()
                );
            })
            .AddScoped<IEnrollUserInCourseQuery, EnrollUserInCourseQuery>(serviceProvider =>
            {
                return new EnrollUserInCourseQuery(
                    serviceProvider.GetRequiredService<DbProviderFactory>(),
                    _connectionString,
                    serviceProvider.GetRequiredService<ILogger<EnrollUserInCourseQuery>>()
                );
            });
    }

    private static IServiceCollection AddAdoServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        string provider = configuration["Persistence:Provider"]!;

        DbProviderFactories.RegisterFactory(provider, MySql.Data.MySqlClient.MySqlClientFactory.Instance);

        return services.AddScoped(serviceProvider =>
        {
            return DbProviderFactories.GetFactory(provider);
        });
    }

    private static WebApplication ApplyMigrations(this WebApplication app)
    {
        IServiceProvider serviceProvider = app.Services.CreateScope().ServiceProvider;
        DbProviderFactory factory = serviceProvider.GetRequiredService<DbProviderFactory>();
        string connectionString = serviceProvider.GetRequiredService<IConfiguration>()["Persistence:ConnectionString"]!;

        using DbConnection connection = factory.CreateConnection()!;
        connection.ConnectionString = connectionString;
        connection.Open();

        string scriptPath = Path.Combine(
            AppContext.BaseDirectory,
            "Persistence/Scripts",
            "create_database.sql"
        );

        using DbCommand command = connection.CreateCommand();
        command.CommandText = File.ReadAllText(scriptPath);
        command.ExecuteNonQuery();
        return app;
    }
}
