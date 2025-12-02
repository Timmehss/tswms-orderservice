using Dapr.Workflow;
using TSWMS.OrderService.Shared.Interfaces;
using TSWMS.OrderService.Shared.Models.DTOs;

namespace TSWMS.OrderService.Business.Workflows.Activities;

public class RestoreProductStockActivity : WorkflowActivity<List<CreateOrderItemDto>, bool>
{
    private readonly IProductService _productService;

    public RestoreProductStockActivity(IProductService productService)
    {
        _productService = productService;
    }

    public override async Task<bool> RunAsync(
        WorkflowActivityContext context,
        List<CreateOrderItemDto> orderItems)
    {
        Console.WriteLine("[RestoreProductStockActivity] Restoring stock for items:");

        var updates = orderItems.Select(i =>
        {
            Console.WriteLine($"[RestoreProductStockActivity] - ProductId: {i.ProductId}, Qty: {i.Quantity}");
            return new UpdateProductStockDto
            {
                ProductId = i.ProductId,
                QuantityChange = i.Quantity
            };
        }).ToList();

        var result = await _productService.RestoreStockAsync(updates);

        if (!result.IsSuccess)
        {
            var errorMessage = result.Errors.FirstOrDefault()?.Message ?? "Failed to restore stock";
            Console.WriteLine($"[RestoreProductStockActivity] FAILED: {errorMessage}");
            throw new InvalidOperationException(errorMessage);
        }

        Console.WriteLine("[RestoreProductStockActivity] Stock successfully restored.");
        return true;
    }
}