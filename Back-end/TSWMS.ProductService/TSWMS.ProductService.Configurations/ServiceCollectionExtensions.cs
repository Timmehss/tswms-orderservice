#region Usings

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TSWMS.ProductService.Business.Managers;
using TSWMS.ProductService.Data;
using TSWMS.ProductService.Data.Repositories;
using TSWMS.ProductService.Shared.Interfaces;

#endregion

namespace TSWMS.ProductService.Configurations;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureUserDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        // Get the connection string from the configuration
        var connectionString = configuration.GetConnectionString("ProductServiceDatabase");

        // Configure the DbContext with the retrieved connection string
        services.AddDbContext<ProductsDbContext>(options =>
            options.UseSqlServer(connectionString));

        return services;
    }

    public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }

    public static IServiceCollection ConfigureManagers(this IServiceCollection services)
    {
        services.AddScoped<IProductManager, ProductManager>();

        return services;
    }
}
