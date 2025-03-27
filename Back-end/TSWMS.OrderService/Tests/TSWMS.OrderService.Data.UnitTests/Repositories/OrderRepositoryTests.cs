#region Usings

using TSWMS.OrderService.Data.Repositories;
using TSWMS.OrderService.Data.UnitTests.Mock;

#endregion

namespace TSWMS.OrderService.Data.UnitTests.Repositories;

public class OrderRepositoryTests
{
    private readonly OrderRepository _orderRepository;

    public OrderRepositoryTests()
    {
        // Initialize the mock DbContext with test data
        var dbContext = OrdersDbContextMock.GetOrdersDbContext();
        _orderRepository = new OrderRepository(dbContext);
    }

    [Fact]
    public async Task GetOrders_ShouldReturnAllOrdersWithOrderItems()
    {
        // Act
        var orders = await _orderRepository.GetOrders();

        // Assert
        Assert.NotNull(orders);
        Assert.Equal(3, orders.Count());
        Assert.All(orders, order =>
            Assert.NotEmpty(order.OrderItems));

        // Optionally, verify the specific contents for one order
        var order = orders.FirstOrDefault(o => o.OrderId == Guid.Parse("a3c99b75-b0a5-4a3b-9c8c-34eed285f269"));
        Assert.NotNull(order);
        Assert.Equal(4, order.OrderItems.Count());
    }
}