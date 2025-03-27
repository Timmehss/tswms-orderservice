using Newtonsoft.Json;
using TSWMS.OrderService.Api.Dto;

namespace TSWMS.OrderService.Api.IntegrationTests;

public class OrderControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public OrderControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetOrders_ReturnsOkWithOrders()
    {
        // Arrange & Act
        var response = await _client.GetAsync("/api/orders");

        // Assert
        response.EnsureSuccessStatusCode();
        var stringResponse = await response.Content.ReadAsStringAsync();
        var orders = JsonConvert.DeserializeObject<List<OrderDto>>(stringResponse);

        Assert.NotNull(orders);
        Assert.NotEmpty(orders);
    }
}
