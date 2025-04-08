#region Usings

using Microsoft.EntityFrameworkCore;
using TSWMS.OrderService.Api.MappingProfiles;
using TSWMS.OrderService.Configurations;
using TSWMS.OrderService.Data;

#endregion

namespace TSWMS.OrderService.Api;
public class Program
{
    public static void Main(string[] args)
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

        // Additional service registrations
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

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
