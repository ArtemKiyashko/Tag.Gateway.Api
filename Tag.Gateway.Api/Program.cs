using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Tag.Gateway.Managers.Infrastructure;
using Microsoft.Extensions.Configuration;
using Tag.Gateway.Api;

IConfiguration _functionConfig;
ServiceBusOptions _servicebusOptions = new();

_functionConfig = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .Build();

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services => {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddMvc().AddNewtonsoftJson();

        _functionConfig.GetSection(nameof(ServiceBusOptions)).Bind(_servicebusOptions);

        if (!string.IsNullOrEmpty(_servicebusOptions.ServiceBusNamespace))
            services.AddMessageManagerManagedIdentity(_servicebusOptions.TopicName, _servicebusOptions.ServiceBusNamespace);
        else
        {
            if (string.IsNullOrEmpty(_servicebusOptions.ServiceBusConnectionString))
                throw new ArgumentException($"{nameof(_servicebusOptions.ServiceBusNamespace)} or {nameof(_servicebusOptions.ServiceBusConnectionString)} required");

            services.AddMessageManagerConnectionString(_servicebusOptions.TopicName, _servicebusOptions.ServiceBusConnectionString);
        }
    })
    .Build();

host.Run();
