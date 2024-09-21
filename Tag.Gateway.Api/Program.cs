using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Tag.Gateway.Managers.Infrastructure;
using Microsoft.Extensions.Configuration;
using Tag.Gateway.Api;

IConfiguration _functionConfig;
QueuesOptions _queuesOptions = new();

_functionConfig = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .Build();

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services => {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddMvc().AddNewtonsoftJson();

        _functionConfig.GetSection(nameof(QueuesOptions)).Bind(_queuesOptions);

        if (_queuesOptions.QueueAccountUri is not null)
            services.AddQueueManager(_queuesOptions.QueueName, _queuesOptions.QueueAccountUri);
        else
        {
            if (string.IsNullOrEmpty(_queuesOptions.QueueAccountConnectionString))
                throw new ArgumentException($"{nameof(_queuesOptions.QueueAccountUri)} or {nameof(_queuesOptions.QueueAccountConnectionString)} required");

            services.AddQueueManager(_queuesOptions.QueueName, _queuesOptions.QueueAccountConnectionString);
        }
    })
    .Build();

host.Run();
