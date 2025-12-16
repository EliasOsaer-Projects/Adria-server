using Adria.Domain.PushNotifications;
using Adria.Infrastructure.PushNotifications;
using Lib.Net.Http.WebPush;
using Lib.Net.Http.WebPush.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Adria.Main.Modules.PushNotifications;

public static class PushNotificationModule
{
    public static IServiceCollection AddPushNotifications(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        IConfigurationSection section = configuration.GetSection("PushNotifications");

        string? subject = section["Subject"];
        string? publicKey = section["PublicKey"];
        string? privateKey = section["PrivateKey"];

        services.AddSingleton(serviceProvider =>
        {
            var client = new PushServiceClient();

            if (!string.IsNullOrWhiteSpace(publicKey) &&
                !string.IsNullOrWhiteSpace(privateKey))
            {
                var authentication = new VapidAuthentication(publicKey, privateKey);
                if (!string.IsNullOrWhiteSpace(subject))
                {
                    authentication.Subject = subject;
                }

                client.DefaultAuthentication = authentication;
            }

            return client;
        });

        services.AddScoped<IPushNotificationSender, PushNotificationSender>();

        return services;
    }
}
