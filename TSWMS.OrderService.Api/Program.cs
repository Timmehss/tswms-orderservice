#region Usings

using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Prometheus;
using RabbitMQ.Client;
using System.Text.Json;
using TSWMS.OrderService.Api.MappingProfiles;
using TSWMS.OrderService.Api.Middlewares;
using TSWMS.OrderService.Configurations;
using TSWMS.OrderService.Data;


//using TSWMS.OrderService.Data.Requesters;

#endregion

namespace TSWMS.OrderService.Api;
public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Get Environment
        var environment = builder.Environment.EnvironmentName;

        Console.WriteLine($"Initial environment: {environment}");

        // Configure App Configuration
        builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);

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
        builder.Services.ConfigureUserDbContext(builder.Configuration, environment);

        // Configure dependency injection for managers and repositories
        builder.Services.ConfigureManagers();
        builder.Services.ConfigureRepositories();

        builder.Services.AddSingleton<IConnectionFactory>(_ =>
        {
            var factory = new ConnectionFactory
            {
                HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost",
                UserName = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? "guest",
                Password = Environment.GetEnvironmentVariable("RABBITMQ_PASS") ?? "guest"
            };
            return factory;
        });

        // Configure FluentValidation
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderDtoValidator>();

        // Register RabbitMQ Publisher/Requester
        //builder.Services.AddSingleton<IProductPriceRequester, ProductPriceRequester>();
        //builder.Services.AddSingleton<IUpdateProductStockRequester, UpdateProductStockRequester>();

        // Additional service registrations
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        //string? secretKey;

        //if (environment == "Test" || environment == "Docker" || environment == "Production" || environment == "Development" || environment == "Kubernetes")
        //{
        //    // Set the key only if it's not already set
        //    secretKey = Environment.GetEnvironmentVariable("HMAC_SECRET_KEY");

        //    if (string.IsNullOrEmpty(secretKey))
        //    {
        //        secretKey = "qWX4IlPFoIKLeSoiiT1JBAl7KvzIRwVm";
        //        Environment.SetEnvironmentVariable("HMAC_SECRET_KEY", secretKey);
        //    }
        //}
        //else
        //{
        //    // Sign RabbitMQ messages with HMAC.
        //    secretKey = Environment.GetEnvironmentVariable("HMAC_SECRET_KEY");
        //}

        //if (string.IsNullOrEmpty(secretKey))
        //{
        //    throw new InvalidOperationException("HMAC secret key is missing!");
        //}

        //builder.Services.Configure<HmacOptions>(options =>
        //{
        //    options.SecretKey = secretKey!;
        //});

        var app = builder.Build();

        // Initialize RabbitMQ Publisher/Requester within async context
        //using (var scope = app.Services.CreateScope())
        //{
        //    var services = scope.ServiceProvider;
        //    var productPriceRequester = services.GetRequiredService<IProductPriceRequester>();
        //    var updateStockRequester = services.GetRequiredService<IUpdateProductStockRequester>();

        //    try
        //    {
        //        await productPriceRequester.InitializeAsync();
        //        await updateStockRequester.InitializeAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the error if RabbitMQ initialization fails
        //        app.Logger.LogError(ex, "Error occurred while initializing RabbitMQ.");
        //    }
        //}

        // Apply Database Migrations if it's not in "Test" environment
        if (environment != "Test" || environment == "Docker" || environment == "Production" || environment == "Kubernetes")
        {
            Console.WriteLine($"Database.Migrate() method | Environment: {environment}");
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

        // Swagger setup for development or Docker
        if (app.Environment.IsDevelopment() || environment == "Docker" || environment == "Production" || environment == "Kubernetes")
        {
            Console.WriteLine($"UseSwagger environment: {environment}");

            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Enable HTTPS redirection and authorization
        app.UseHttpsRedirection();
        app.UseAuthorization();

        // Map controllers to endpoints
        app.MapControllers();

        app.UseHttpMetrics();
        app.MapMetrics();

        // Run the application
        app.Run();
    }
}
