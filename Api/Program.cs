using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Reflectionnaire.Api.DataAccess;
using Reflectionnaire.Api.Options;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddOptions<ReflectionnaireOptions>().Configure<IConfiguration>((settings, configuration) =>
        {
            configuration.GetSection(nameof(ReflectionnaireOptions)).Bind(settings);
        });
        services.AddTransient<ITableClientFactory, TableClientFactory>();
    })
    .Build();

host.Run();
