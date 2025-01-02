using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Reflectionnaire.Api.DataAccess;
using Reflectionnaire.Api.Options;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights()
    .AddTransient<ITableClientFactory, TableClientFactory>()
    .AddOptions<ReflectionnaireOptions>().Configure<IConfiguration>((settings, configuration) =>
    {
        configuration.GetSection(nameof(ReflectionnaireOptions)).Bind(settings);
    });

builder.Build().Run();

