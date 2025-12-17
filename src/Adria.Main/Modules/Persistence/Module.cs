using Adria.Infrastructure;
using System.Data.Common;
using Adria.Application.Contracts;
using Adria.Domain;
using Adria.Domain.Subscriptions;
using Adria.Domain.Users;
using Adria.Infrastructure.Persistence.Queries;
using Adria.Infrastructure.Persistence.Repositories;
using Taskly.Infrastructure.Persistence.Queries;
using Adria.Domain.PushNotifications;
using Adria.Main.Modules.PushNotifications;

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
            .AddQueries()
            .AddPushNotifications(configuration);
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
        return services
            .AddScoped<IUserRepository, AdoUserRepository>(serviceProvider =>
            {
                return new AdoUserRepository(
                    serviceProvider.GetRequiredService<DbProviderFactory>(),
                    _connectionString,
                    serviceProvider.GetRequiredService<ILogger<AdoUserRepository>>()
                );
            })
            .AddScoped<ISubscriptionRepository, AdoSubscriptionRepository>(serviceProvider =>
            {
                return new AdoSubscriptionRepository(
                    serviceProvider.GetRequiredService<DbProviderFactory>(),
                    _connectionString,
                    serviceProvider.GetRequiredService<ILogger<AdoSubscriptionRepository>>()
                );
            })
            .AddScoped<IUserCourseRepository, AdoUserCourseRepository>(serviceProvider =>
            {
                return new AdoUserCourseRepository(
                    serviceProvider.GetRequiredService<DbProviderFactory>(),
                    _connectionString,
                    serviceProvider.GetRequiredService<ILogger<AdoUserCourseRepository>>()
                );
            })
            .AddScoped<IUserSubscriptionRepository, AdoUserSubscriptionRepository>(serviceProvider =>
            {
                return new AdoUserSubscriptionRepository(
                    serviceProvider.GetRequiredService<DbProviderFactory>(),
                    _connectionString,
                    serviceProvider.GetRequiredService<ILogger<AdoUserSubscriptionRepository>>(),
                    serviceProvider.GetRequiredService<ISubscriptionRepository>()
                );
            })
            .AddScoped<IUserCourseRepository, AdoUserCourseRepository>(ServiceProvider =>
            {
                return new AdoUserCourseRepository(
                    ServiceProvider.GetRequiredService<DbProviderFactory>(),
                    _connectionString,
                    ServiceProvider.GetRequiredService<ILogger<AdoUserCourseRepository>>()
                    );
            })
            .AddScoped<IUserRepository, AdoUserRepository>(serviceProvider =>
            {
                return new AdoUserRepository(
                    serviceProvider.GetRequiredService<DbProviderFactory>(),
                    _connectionString,
                    serviceProvider.GetRequiredService<ILogger<AdoUserRepository>>()
                );
            })
            .AddScoped<IUserQARepository, AdoUserQARepository>(serviceProvider =>
            {
                return new AdoUserQARepository(
                    serviceProvider.GetRequiredService<DbProviderFactory>(),
                    _connectionString,
                    serviceProvider.GetRequiredService<ILogger<AdoUserQARepository>>()
                );
            })
            .AddScoped<IPushSubscriptionRepository, AdoPushSubscriptionRepository>(serviceProvider =>
            {
                return new AdoPushSubscriptionRepository(
                    serviceProvider.GetRequiredService<DbProviderFactory>(),
                    _connectionString,
                    serviceProvider.GetRequiredService<ILogger<AdoPushSubscriptionRepository>>()
                );
            });
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
            .AddScoped<IActivePromoCodesQuery, CurrentPromoCodesQuery>(serviceProvider =>
            {
                return new CurrentPromoCodesQuery(serviceProvider.GetRequiredService<DbProviderFactory>(),
                    _connectionString,
                    serviceProvider.GetRequiredService<ILogger<CurrentPromoCodesQuery>>()
                );
            })
            .AddScoped<ICourseModulesByCourseIdQuery, CourseModulesByCourseIdQuery>(serviceProvider =>
            {
                return new CourseModulesByCourseIdQuery(
                    serviceProvider.GetRequiredService<DbProviderFactory>(),
                    _connectionString,
                    serviceProvider.GetRequiredService<ILogger<CourseModulesByCourseIdQuery>>()
                );
            })
            .AddScoped<IAllCosmeticsQuery, AllCosmeticsQuery>(serviceProvider =>
            {
                return new AllCosmeticsQuery(
                    serviceProvider.GetRequiredService<DbProviderFactory>(),
                    _connectionString,
                    serviceProvider.GetRequiredService<ILogger<AllCosmeticsQuery>>()
                    );
            })
            .AddScoped<IAllOccupationsQuery, AllOccupationsQuery>(serviceProvider =>
            {
                return new AllOccupationsQuery(
                    serviceProvider.GetRequiredService<DbProviderFactory>(),
                    _connectionString,
                    serviceProvider.GetRequiredService<ILogger<AllOccupationsQuery>>()
                );
            })
            .AddScoped<IAllInterestsQuery, AllInterestsQuery>(serviceProvider =>
            {
                return new AllInterestsQuery(
                    serviceProvider.GetRequiredService<DbProviderFactory>(),
                    _connectionString,
                    serviceProvider.GetRequiredService<ILogger<AllInterestsQuery>>()
                );
            })
            .AddScoped<IGetNextCourseModuleQuery, GetNextCourseModuleQuery>(serviceProvider =>
            {
                return new GetNextCourseModuleQuery(
                    serviceProvider.GetRequiredService<DbProviderFactory>(),
                    _connectionString,
                    serviceProvider.GetRequiredService<ILogger<GetNextCourseModuleQuery>>()
                );
            })
            .AddScoped<IDiscountByPromoCodeQuery, DiscountByPromoCodeQuery>(serviceProvider =>
            {
                return new DiscountByPromoCodeQuery(
                    serviceProvider.GetRequiredService<DbProviderFactory>(),
                    _connectionString,
                    serviceProvider.GetRequiredService<ILogger<DiscountByPromoCodeQuery>>()
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

        SeedCoverAndBannerImages(connection);
        SeedProfilePictures(connection);

        return app;
    }

    private static void SeedProfilePictures(DbConnection connection)
    {
        var users = new List<(string AdriaID, string Name)>();
        using (DbCommand selectCmd = connection.CreateCommand())
        {
            selectCmd.CommandText = "SELECT AdriaID, Name FROM User";
            using (var reader = selectCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    users.Add((reader.GetString(0), reader.GetString(1)));
                }
            }
        }

        foreach (var user in users)
        {
            string coverImgPath = Path.Combine(
                AppContext.BaseDirectory,
                "Persistence/Scripts/SeedImages/ProfilePictures",
                $"{user.Name}.jpg"
            );

            using (var imageCommand = connection.CreateCommand())
            {
                imageCommand.Parameters.Add(CreateParam(imageCommand, "@id", user.AdriaID));
                imageCommand.Parameters.Add(CreateParam(imageCommand, "@img", File.ReadAllBytes(coverImgPath)));

                imageCommand.CommandText = @"
                    UPDATE User 
                    SET ProfilePicture = @img
                    WHERE AdriaID = @id                
                ";

                imageCommand.ExecuteNonQuery();
            }
        }
    }

    private static void SeedCoverAndBannerImages(DbConnection connection)
    {
        // Seed images
        var courseIds = new List<(string CourseId, string CourseName)>();
        using (DbCommand selectCmd = connection.CreateCommand())
        {
            selectCmd.CommandText = "SELECT CourseID, CourseName FROM Course";
            using (var reader = selectCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    courseIds.Add((reader.GetString(0), reader.GetString(1)));
                }
            }
        }

        foreach (var course in courseIds)
        {
            string coverImageId = Guid.NewGuid().ToString();
            string bannerImageId = Guid.NewGuid().ToString();

            string coverImgPath = Path.Combine(
                AppContext.BaseDirectory,
                "Persistence/Scripts/SeedImages/Covers",
                $"{course.CourseName}.jpg"
            );
            string bannerImgPath = Path.Combine(
                AppContext.BaseDirectory,
                "Persistence/Scripts/SeedImages/Banners",
                $"{course.CourseName}.jpg"
            );

            using (var imageCommand = connection.CreateCommand())
            {
                imageCommand.CommandText = @"
                INSERT INTO CoverImage (CoverImageID, CoverImage) 
                VALUES (@id, @img)
                ";

                imageCommand.Parameters.Add(CreateParam(imageCommand, "@id", coverImageId));
                imageCommand.Parameters.Add(CreateParam(imageCommand, "@img", File.ReadAllBytes(coverImgPath)));

                imageCommand.ExecuteNonQuery();
                imageCommand.Parameters.Clear();

                imageCommand.CommandText = @"
                INSERT INTO BannerImage (BannerImageID, BannerImage) 
                VALUES (@id, @img)
                ";

                imageCommand.Parameters.Add(CreateParam(imageCommand, "@id", bannerImageId));
                imageCommand.Parameters.Add(CreateParam(imageCommand, "@img", File.ReadAllBytes(bannerImgPath)));

                imageCommand.ExecuteNonQuery();
                imageCommand.Parameters.Clear();
            }

            using (var updateCourseForeignKeys = connection.CreateCommand())
            {
                updateCourseForeignKeys.CommandText = @"
                    UPDATE Course 
                    SET CoverImageID = @coverID, 
                        BannerImageID = @bannerID 
                    WHERE CourseID = @courseID
                ";

                updateCourseForeignKeys.Parameters.Add(CreateParam(updateCourseForeignKeys, "@coverID", coverImageId));
                updateCourseForeignKeys.Parameters.Add(CreateParam(updateCourseForeignKeys, "@bannerID", bannerImageId));
                updateCourseForeignKeys.Parameters.Add(CreateParam(updateCourseForeignKeys, "@courseID", course.CourseId));

                updateCourseForeignKeys.ExecuteNonQuery();
            }
        }

    }

    private static DbParameter CreateParam(DbCommand cmd, string name, object value)
    {
        var param = cmd.CreateParameter();
        param.ParameterName = name;
        param.Value = value;
        return param;
    }


}
