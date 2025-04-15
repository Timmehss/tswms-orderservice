using Moq;
using Newtonsoft.Json;
using System.Net;
using TSWMS.OrderService.Api.Dto;
using TSWMS.OrderService.Shared.Models;
using TSWMS.OrderService.Shared.Testing;

namespace TSWMS.OrderService.Api.IntegrationTests.Controllers;

public class OrderControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public OrderControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
        _factory = factory;
    }

    [Fact]
    public async Task GetOrders_ReturnsOkWithOrders()
    {
        // Arrange
        var fakeOrders = TestDataHelper.GetFakeOrders();
        _factory.OrderManagerMock.Setup(m => m.GetOrdersAsync()).ReturnsAsync(fakeOrders);

        // Act
        var response = await _client.GetAsync("/api/orders");

        // Assert
        response.EnsureSuccessStatusCode();
        var stringResponse = await response.Content.ReadAsStringAsync();
        var orders = JsonConvert.DeserializeObject<List<OrderDto>>(stringResponse);

        Assert.NotNull(orders);
        Assert.NotEmpty(orders);
        Assert.Equal(2, orders.Count);

        foreach (var order in orders)
        {
            Assert.Equal(3, order.OrderItems.Count);
        }
    }

    [Fact]
    public async Task GetOrders_ReturnsNotFoundWhenNoOrders()
    {
        // Arrange
        _factory.OrderManagerMock.Setup(m => m.GetOrdersAsync()).ReturnsAsync(new List<Order>());

        // Act
        var response = await _client.GetAsync("/api/orders");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var stringResponse = await response.Content.ReadAsStringAsync();
        Assert.Contains("No orders found.", stringResponse);
    }

    [Fact]
    public async Task GetOrders_ReturnsNotFoundWhenOrdersNull()
    {
        // Arrange
        _factory.OrderManagerMock.Setup(m => m.GetOrdersAsync()).ReturnsAsync((List<Order>)null);

        // Act
        var response = await _client.GetAsync("/api/orders");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var stringResponse = await response.Content.ReadAsStringAsync();
        Assert.Contains("No orders found.", stringResponse);
    }

    [Fact]
    public async Task GetOrders_HandlesExceptionAndReturnsServerError()
    {
        // Arrange
        _factory.OrderManagerMock.Setup(m => m.GetOrdersAsync()).ThrowsAsync(new Exception("test exception"));

        // Act
        var response = await _client.GetAsync("/api/orders");

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        var stringResponse = await response.Content.ReadAsStringAsync();
        Assert.Contains("An error occurred: test exception", stringResponse);
    }
}
