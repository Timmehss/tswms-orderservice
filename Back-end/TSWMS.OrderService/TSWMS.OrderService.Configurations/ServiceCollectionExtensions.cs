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
        public static IServiceCollection ConfigureUserDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            // Get the connection string from the configuration
            var connectionString = configuration.GetConnectionString("OrderServiceDatabase");

            // Configure the DbContext with the retrieved connection string
            services.AddDbContext<OrdersDbContext>(options =>
                options.UseSqlServer(connectionString));

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

