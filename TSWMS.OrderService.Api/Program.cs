#region Usings

using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TSWMS.OrderService.Api.MappingProfiles;
using TSWMS.OrderService.Api.Middlewares;
using TSWMS.OrderService.Configurations;
using TSWMS.OrderService.Data;
using TSWMS.OrderService.Shared.Interfaces;

#endregion

namespace TSWMS.OrderService.Api;
public class Program
{
    public static async Task Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        // Get Environment
        var environment = builder.Environment.EnvironmentName;

        // Configure App Configuration
        builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);

        // Add Cors Policie
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

        // Configure dependency injection for managers
        builder.Services.ConfigureManagers();

        // Configure dependency injection for repositories
        builder.Services.ConfigureRepositories();

        // Configure FluentValidation
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderDtoValidator>();

        // Register RabbitMQ Publisher
        builder.Services.AddSingleton<IRabbitMqPublisher, RabbitMqPublisher>();

        // Additional service registrations
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                // Apply camelCase to api responses
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Initialize RabbitMQ Publisher
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var rabbitMqPublisher = services.GetRequiredService<IRabbitMqPublisher>();
            await rabbitMqPublisher.InitAsync();
        }

        if (environment != "Test")
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var dbContext = services.GetRequiredService<OrdersDbContext>();

                // Apply pending migrations or create database if it doesn't exist
                dbContext.Database.Migrate();
            }
        }

        app.UseCors("TSWMSPolicy");

        app.UseMiddleware<ExceptionHandlingMiddleware>();


        if (app.Environment.IsDevelopment() || environment == "Docker")
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}
