using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Toxiproxy.Net.Toxics;
using TSWMS.IntegrationTests;
using TSWMS.OrderService.Shared.Models.DTOs;

namespace TSWMS.OrderService.DaprTests;

public class GetPricesResilienceTests : IntegrationTestBase
{
    [Fact]
    public async Task CreateOrder_WhenProductServiceIsSlow_ShouldRetryAndEventuallyFail()
    {
        // 1. ARRANGE
        var randomProductId = Guid.NewGuid();

        var request = new CreateOrderDto
        {
            OrderItems = new List<CreateOrderItemDto>
            {
                new CreateOrderItemDto { ProductId = randomProductId, Quantity = 10 }
            }
        };

        var proxy = await GetProductProxyAsync();

        var latencyToxic = new LatencyToxic();
        latencyToxic.Name = "network_lag";
        latencyToxic.Attributes.Latency = 5000;
        latencyToxic.Attributes.Jitter = 0;

        await proxy.AddAsync(latencyToxic);

        // 2. ACT
        Console.WriteLine("[TEST] Sending Create Order Request...");
        var response = await OrderClient.PostAsJsonAsync("api/orders", request);

        response.StatusCode.Should().Be(HttpStatusCode.Accepted);

        // FIX 1: Use a real class instead of dynamic
        var responseBody = await response.Content.ReadFromJsonAsync<CreateOrderTestResponse>();

        // Note: The property is capitalized in the class, JSON serializer handles the case conversion automatically
        string statusUrl = responseBody!.CheckStatusUrl;

        Console.WriteLine($"[TEST] Workflow started. Polling status at: {statusUrl}");

        // 3. ASSERT
        string finalStatus = "Pending";
        string? errorMessage = null;

        for (int i = 0; i < 25; i++)
        {
            await Task.Delay(1000);

            var statusRes = await OrderClient.GetAsync(statusUrl);

            // FIX 2: Use a real class here too
            var statusData = await statusRes.Content.ReadFromJsonAsync<WorkflowStatusTestResponse>();

            string currentStatus = statusData!.RuntimeStatus;

            if (currentStatus != "Pending" && currentStatus != "Running")
            {
                finalStatus = currentStatus;
                errorMessage = statusData.Error;
                break;
            }
        }

        finalStatus.Should().Be("Failed", because: "The workflow should exhaust retries due to network lag");

        Console.WriteLine($"[TEST] Workflow finished with status: {finalStatus}");
        Console.WriteLine($"[TEST] Error Message: {errorMessage}");
    }
}

// Maps the anonymous object returned by your OrderController.CreateOrder
internal class CreateOrderTestResponse
{
    public string WorkflowInstanceId { get; set; }
    public string Status { get; set; }
    public string CheckStatusUrl { get; set; }
}

// Maps the Dapr Workflow status response
internal class WorkflowStatusTestResponse
{
    public string RuntimeStatus { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    public string Error { get; set; }
}