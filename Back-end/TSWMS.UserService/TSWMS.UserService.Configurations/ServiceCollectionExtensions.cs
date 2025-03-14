#region Usings

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TSWMS.UserService.Business.Services;
using TSWMS.UserService.Shared.Interfaces;
using UserService.Data;
using UserService.Data.Repositories;
using UserService.Shared.Interfaces;

#endregion

namespace UserService.Configurations
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureUserDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            // Get the connection string from the configuration
            var connectionString = configuration.GetConnectionString("UserServiceDatabase");

            // Configure the DbContext with the retrieved connection string
            services.AddDbContext<UserDbContext>(options =>
                options.UseSqlServer(connectionString));

            return services;
        }

        public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }

        public static IServiceCollection ConfigureManagers(this IServiceCollection services)
        {
            services.AddScoped<IUserManager, UserManager>();

            return services;
        }
    }
}
