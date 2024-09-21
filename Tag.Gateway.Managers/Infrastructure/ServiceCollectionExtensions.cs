using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Tag.Gateway.Repositories;

namespace Tag.Gateway.Managers.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMessageManagerConnectionString(this IServiceCollection services, string topicName, string messageAccountConnectionString)
    {
        services.AddAzureClients(clientBuilder => {
            clientBuilder.AddServiceBusClient(messageAccountConnectionString);
            clientBuilder
                .AddClient<ServiceBusSender, ServiceBusClientOptions>((_, _, provider) => provider.GetRequiredService<ServiceBusClient>().CreateSender(topicName));
        });

        services.AddBusinessogic();

        return services;
    }

    public static IServiceCollection AddMessageManagerManagedIdentity(this IServiceCollection services, string topicName, string messageAccountNamespace)
    {
        services.AddAzureClients(clientBuilder =>
        {
            clientBuilder.UseCredential(new ManagedIdentityCredential());
            clientBuilder.AddServiceBusClientWithNamespace(messageAccountNamespace);
            clientBuilder
                .AddClient<ServiceBusSender, ServiceBusClientOptions>((_, _, provider) => provider.GetRequiredService<ServiceBusClient>().CreateSender(topicName));
        });

        services.AddBusinessogic();

        return services;
    }

    private static void AddBusinessogic(this IServiceCollection services)
    {
        services.AddScoped<IMessageManager, MessageManager>();
        services.AddScoped<IMessageRepository, MessageRepository>();
    }
}
