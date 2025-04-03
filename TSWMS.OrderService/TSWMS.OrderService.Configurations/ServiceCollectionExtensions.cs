#region Usings

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TSWMS.OrderService.Business.Managers;
using TSWMS.OrderService.Data;
using TSWMS.OrderService.Data.Repositories;
using TSWMS.OrderService.Shared.Interfaces;

#endregion

namespace TSWMS.OrderService.Configurations
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureUserDbContext(this IServiceCollection services, IConfiguration configuration, string environment)
        {
            if (environment == "Test")
            {
                // Use an in-memory database for testing
                services.AddDbContext<OrdersDbContext>(options =>
                    options.UseInMemoryDatabase("InMemoryOrderServiceDbForTesting"));
            }
            else
            {
                // Use SQL Server for production or development
                var connectionString = configuration.GetConnectionString("OrderServiceDatabase");
                services.AddDbContext<OrdersDbContext>(options =>
                    options.UseSqlServer(connectionString));
            }

            return services;
        }


        public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IOrderRepository, OrderRepository>();

            return services;
        }

        public static IServiceCollection ConfigureManagers(this IServiceCollection services)
        {
            services.AddScoped<IOrderManager, OrderManager>();

            return services;
        }
    }
}

