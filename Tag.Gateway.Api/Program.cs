using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Tag.Gateway.Managers.Infrastructure;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

IConfiguration _functionConfig;

_functionConfig = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .Build();

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services => {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddMvc().AddNewtonsoftJson();

        var storageAccountUrl = _functionConfig.GetValue<string>("StorageAccountUrl");
        var queueName = _functionConfig.GetValue<string>("QueueName");

        if (string.IsNullOrEmpty(queueName))
            throw new ArgumentException("QueueName cannot be empty");

        if (!string.IsNullOrEmpty(storageAccountUrl))
            services.AddQueueManager(queueName, new Uri(storageAccountUrl));
        else
        {
            var storageAccountConnectionString = _functionConfig.GetValue<string>("StorageAccountConnectionString");
            
            if (string.IsNullOrEmpty(storageAccountConnectionString))
                throw new ArgumentException("StorageAccountUrl or StorageAccountConnectionString required");

            services.AddQueueManager(queueName, storageAccountConnectionString);
        }
    })
    .Build();

host.Run();
