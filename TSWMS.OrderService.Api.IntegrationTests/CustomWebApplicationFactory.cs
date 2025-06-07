#region Usings

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using TSWMS.OrderService.Data;
using TSWMS.OrderService.Shared.Interfaces;

#endregion

namespace TSWMS.OrderService.Api.IntegrationTests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        public Mock<IOrderManager> OrderManagerMock { get; } = new Mock<IOrderManager>();
        public Mock<IProductPriceRequester> RabbitMqPublisherMock { get; } = new Mock<IProductPriceRequester>();

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseEnvironment("Test");

            builder.ConfigureServices(services =>
            {
                // Remove the app's OrdersDbContext registration.
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<OrdersDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add a database context (OrdersDbContext) using an in-memory database for testing.
                services.AddDbContext<OrdersDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestUserServiceDb");
                });

                // Remove IOrderManager if it exists and add the mock
                var orderManagerDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IOrderManager));
                if (orderManagerDescriptor != null)
                {
                    services.Remove(orderManagerDescriptor);
                }
                services.AddSingleton<IOrderManager>(OrderManagerMock.Object);

                // Remove IRabbitMqPublisher if it exists and add the mock
                var rabbitMqPublisherDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IProductPriceRequester));

                if (rabbitMqPublisherDescriptor != null)
                {
                    services.Remove(rabbitMqPublisherDescriptor);
                }
                services.AddSingleton<IProductPriceRequester>(RabbitMqPublisherMock.Object);

                // Ensure the database is created using the in-memory configuration.
                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<OrdersDbContext>();
                    db.Database.EnsureCreated();
                }
            });

            return base.CreateHost(builder);
        }
    }
}