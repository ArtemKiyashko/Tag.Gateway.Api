using Azure.Identity;
using Azure.Storage.Queues;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Tag.Gateway.Repositories;

namespace Tag.Gateway.Managers.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQueueManager(this IServiceCollection services, string queueName, string storageAccountConnectionString)
    {
        services.AddAzureClients(clientBuilder => {
            clientBuilder.UseCredential(new DefaultAzureCredential());
            clientBuilder
                .AddQueueServiceClient(storageAccountConnectionString)
                .ConfigureOptions((options) => { options.MessageEncoding = QueueMessageEncoding.Base64;});
        });

        services.AddQueueClient(queueName);

        return services;
    }

    public static IServiceCollection AddQueueManager(this IServiceCollection services, string queueName, Uri storageAccountUri)
    {
        services.AddAzureClients(clientBuilder =>
        {
            clientBuilder.UseCredential(new DefaultAzureCredential());
            clientBuilder
                .AddQueueServiceClient(storageAccountUri)
                .ConfigureOptions((options) => { options.MessageEncoding = QueueMessageEncoding.Base64; });
        });

        services.AddQueueClient(queueName);

        return services;
    }

    private static void AddQueueClient(this IServiceCollection services, string queueName)
    {
        services.AddScoped<QueueClient>((factory) =>
        {
            var service = factory.GetRequiredService<QueueServiceClient>();
            var client = service.GetQueueClient(queueName);
            client.CreateIfNotExists();
            return client;
        });

        AddBusinessogic(services);
    }

    private static void AddBusinessogic(this IServiceCollection services)
    {
        services.AddScoped<IQueueManager, QueueManager>();
        services.AddScoped<IQueueRepository, QueueRepository>();
    }
}
