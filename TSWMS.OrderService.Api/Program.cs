#region Usings

using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using System.Text.Json;
using TSWMS.OrderService.Api.MappingProfiles;
using TSWMS.OrderService.Api.Middlewares;
using TSWMS.OrderService.Business.Managers;
using TSWMS.OrderService.Configurations;
using TSWMS.OrderService.Data;
using TSWMS.OrderService.Data.Clients;
using TSWMS.OrderService.Shared.Interfaces;
using TSWMS.OrderService.Shared.Interfaces.Clients;
using TSWMS.OrderService.Shared.Options;

#endregion

namespace TSWMS.OrderService.Api;
public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Get Environment
        var environment = builder.Environment;

        Console.WriteLine($"Initial environment: {environment}");

        // Configure App Configuration
        builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);

        // Configure Dapr Services & Endpoints
        builder.Configuration.AddJsonFile("dapr.services.json", optional: false, reloadOnChange: true);
        builder.Configuration.AddJsonFile("dapr.config.json", optional: false, reloadOnChange: true);

        // Add Dapr
        builder.Services.AddDaprClient();

        // Add Cors Policy
        builder.Services.AddCors(o => o.AddPolicy("TSWMSPolicy", builder =>
        {
            builder.SetIsOriginAllowed((host) => true)
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
        }));

        // Configure AutoMapper Profiles
        builder.Services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<OrderMappingProfile>();
        });

        // Configure EntityFramework UserDbContext
        builder.Services.ConfigureUserDbContext(builder.Configuration, environment.EnvironmentName);

        // Configure dependency injection for managers and repositories
        builder.Services.ConfigureManagers();
        builder.Services.ConfigureRepositories();

        // Configure FluentValidation
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderDtoValidator>();

        // Additional service registrations
        builder.Services.AddControllers()
            .AddDapr()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddScoped<IOrderManager, OrderManager>();

        builder.Services.AddScoped<IProductClient, ProductClient>();

        string? secretKey;

        if (environment.IsDevelopment() || environment.IsEnvironment("Test") || environment.IsEnvironment("Docker") || environment.IsEnvironment("Production") || environment.IsEnvironment("Kubernetes"))
        {
            // Set the key only if it's not already set
            secretKey = Environment.GetEnvironmentVariable("HMAC_SECRET_KEY");

            if (string.IsNullOrEmpty(secretKey))
            {
                secretKey = "qWX4IlPFoIKLeSoiiT1JBAl7KvzIRwVm";
                Environment.SetEnvironmentVariable("HMAC_SECRET_KEY", secretKey);
            }
        }
        else
        {
            // Sign RabbitMQ messages with HMAC.
            secretKey = Environment.GetEnvironmentVariable("HMAC_SECRET_KEY");
        }

        if (string.IsNullOrEmpty(secretKey))
        {
            throw new InvalidOperationException("HMAC secret key is missing!");
        }

        builder.Services.Configure<HmacOptions>(options =>
        {
            options.SecretKey = secretKey!;
        });

        builder.Services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddSqlClientInstrumentation()
                    .AddPrometheusExporter();
            });

        var app = builder.Build();

        var logger = app.Services.GetRequiredService<ILogger<Program>>();

        logger.LogInformation("Current environment: {env}", environment.EnvironmentName);

        // Apply Database Migrations if it's not in "Test" environment
        if (environment.IsEnvironment("Docker") || environment.IsEnvironment("Production") || environment.IsEnvironment("Kubernetes"))
        {
            logger.LogInformation("Database.Migrate() method | Environment: {environment}", environment);

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var dbContext = services.GetRequiredService<OrdersDbContext>();

                // Apply pending migrations or create the database if it doesn't exist
                dbContext.Database.Migrate();
            }
        }

        // Use CORS policy
        app.UseCors("TSWMSPolicy");

        // Exception handling middleware
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        // Swagger setup
        if (environment.IsDevelopment() || environment.IsEnvironment("Docker") || environment.IsEnvironment("Production") || environment.IsEnvironment("Kubernetes"))
        {
            logger.LogInformation("UseSwagger triggered for environment: {env}", environment.EnvironmentName);

            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseOpenTelemetryPrometheusScrapingEndpoint();

        app.UseAuthorization();

        app.UseCloudEvents();

        // Map controllers
        app.MapControllers();

        // Add Dapr subscribe handler
        app.MapSubscribeHandler();

        // Run the application
        app.Run();
    }
}
