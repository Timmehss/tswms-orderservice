using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using TSWMS.OrderService.DaprTests.Models;
using TSWMS.OrderService.Shared.Models.DTOs;

namespace TSWMS.OrderService.DaprTests;

public class OrderCompensationTests : IntegrationTestBase
{
    [Fact]
    public async Task CreateOrder_WhenStockServiceDown_ShouldCompensateAndRemoveOrder()
    {
        // 1. WARM-UP PHASE (Populate the Cache)

        var productId = Guid.Parse("7B13656F-8A1F-4BA4-9DFB-340F2E1C362C");
        var proxy = await GetProductProxyAsync();

        // Ensure network is HEALTHY first
        proxy.Enabled = true;
        await proxy.UpdateAsync();

        Console.WriteLine("[TEST] Warming up cache with a successful order...");

        var warmupRequest = new CreateOrderDto
        {
            OrderItems = new List<CreateOrderItemDto>
            {
                new CreateOrderItemDto { ProductId = productId, Quantity = 1 }
            }
        };

        // Run the warmup order
        var warmupResponse = await OrderClient.PostAsJsonAsync("api/orders", warmupRequest);
        warmupResponse.StatusCode.Should().Be(HttpStatusCode.Accepted);

        // Wait for it to complete (so we know the cache is definitely set)
        await WaitForWorkflowCompletion(warmupResponse);

        // 2. SABOTAGE PHASE (Cut the Network)

        Console.WriteLine("[TEST] Cache populated. Cutting connection to ProductService...");

        // Disable the proxy. Any network call to ProductService will now fail immediately.
        proxy.Enabled = false;
        await proxy.UpdateAsync();

        // 3. TEST PHASE (The "Zombie" Order)

        // We use a unique quantity (22) so we can easily find this specific order later
        int uniqueQuantity = 22;

        var request = new CreateOrderDto
        {
            OrderItems = new List<CreateOrderItemDto>
            {
                new CreateOrderItemDto { ProductId = productId, Quantity = uniqueQuantity }
            }
        };

        Console.WriteLine("[TEST] Sending the failing order request...");
        var response = await OrderClient.PostAsJsonAsync("api/orders", request);
        response.StatusCode.Should().Be(HttpStatusCode.Accepted);

        // 4. VERIFY FAILURE

        // Poll until the workflow status is "Failed"
        var finalStatus = await WaitForWorkflowCompletion(response);

        // Assert the workflow failed (because Step 3 Stock Deduction couldn't reach the service)
        finalStatus.RuntimeStatus.Should().Be("Failed");

        // 5. ASSERT COMPENSATION (The Critical Check)

        // If Compensation worked: The order was created (Step 2) but then DELETED.
        // If Compensation failed: The order would still exist in the DB (a "Zombie" order).

        Console.WriteLine("[TEST] Verifying order was deleted from the database...");

        var allOrders = await OrderClient.GetFromJsonAsync<List<OrderDto>>("api/orders");

        // We assert that NO order exists with our unique product and quantity
        var zombieOrder = allOrders!.FirstOrDefault(o =>
            o.OrderItems.Any(i => i.ProductId == productId && i.Quantity == uniqueQuantity));

        zombieOrder.Should().BeNull(because: "The CompensateCreateOrderActivity should have deleted the order after stock deduction failed");
    }

    // Helper to reduce code duplication
    private async Task<WorkflowStatusResponse> WaitForWorkflowCompletion(HttpResponseMessage response)
    {
        var responseBody = await response.Content.ReadFromJsonAsync<CreateOrderTestResponse>();
        string statusUrl = responseBody!.CheckStatusUrl;

        for (int i = 0; i < 30; i++)
        {
            await Task.Delay(1000);
            var statusRes = await OrderClient.GetAsync(statusUrl);
            var statusData = await statusRes.Content.ReadFromJsonAsync<WorkflowStatusResponse>();

            if (statusData!.RuntimeStatus != "Pending" && statusData.RuntimeStatus != "Running")
            {
                return statusData;
            }
        }
        throw new Exception("Workflow timed out in test");
    }
}
