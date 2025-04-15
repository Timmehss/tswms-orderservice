#region Usings

using Moq;
using TSWMS.OrderService.Business.Managers;
using TSWMS.OrderService.Shared.Interfaces;
using TSWMS.OrderService.Shared.Models;

#endregion

namespace TSWMS.OrderService.Business.UnitTests.Managers;

public class OrderManagerTests
{
    private readonly OrderManager _orderManager;
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<IProductPriceRequester> _rabbitMqPublisherMock;

    public OrderManagerTests()
    {
        // Mock the IOrderRepository
        _orderRepositoryMock = new Mock<IOrderRepository>();

        // Mock the IRabbitMqPublisher
        _rabbitMqPublisherMock = new Mock<IProductPriceRequester>();

        // Initialize OrderManager with the mocked repository and mocked publisher
        _orderManager = new OrderManager(_orderRepositoryMock.Object, _rabbitMqPublisherMock.Object);
    }

    [Fact]
    public async Task GetOrders_ShouldReturnOrdersFromRepository()
    {
        // Arrange
        var expectedOrders = new List<Order>
        {
            new Order
            {
                OrderId = Guid.Parse("a3c99b75-b0a5-4a3b-9c8c-34eed285f269"),
                UserId = Guid.Parse("52348777-7a0e-4139-9489-87dff9d47b7e"),
                TotalPrice = 160.00m,
                OrderDate = new DateTime(2024, 7, 15),
                OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        OrderId = Guid.Parse("a3c99b75-b0a5-4a3b-9c8c-34eed285f269"),
                        ProductId = Guid.Parse("de3c9457-f6a8-4b4b-a4c1-f9d3db6f1e1d"),
                        Quantity = 1,
                        UnitPrice = 30.00m
                    }
                }
            }
        };

        _orderRepositoryMock.Setup(repo => repo.GetOrders()).ReturnsAsync(expectedOrders);

        // Act
        var orders = await _orderManager.GetOrdersAsync();

        // Assert
        Assert.NotNull(orders);
        Assert.Equal(expectedOrders.Count, orders.Count());
        var firstOrder = orders.First();
        Assert.NotNull(firstOrder);
        Assert.Equal(expectedOrders.First().OrderId, firstOrder.OrderId);
        Assert.Single(firstOrder.OrderItems);

        var firstOrderItem = firstOrder.OrderItems.First();
        Assert.Equal(expectedOrders.First().OrderItems.First().ProductId, firstOrderItem.ProductId);
        Assert.Equal(expectedOrders.First().OrderItems.First().Quantity, firstOrderItem.Quantity);
        Assert.Equal(expectedOrders.First().OrderItems.First().UnitPrice, firstOrderItem.UnitPrice);

        // Verify that the GetOrders method on the repository was called exactly once
        _orderRepositoryMock.Verify(repo => repo.GetOrders(), Times.Once);
    }
}