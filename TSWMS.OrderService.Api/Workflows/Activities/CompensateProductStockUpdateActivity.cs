using Dapr.Workflow;
using TSWMS.OrderService.Api.Dto;
using TSWMS.OrderService.Shared.Interfaces;
using TSWMS.OrderService.Shared.Models.DTOs;

namespace TSWMS.OrderService.Api.Workflows.Activities;

public class CompensateProductStockUpdateActivity : WorkflowActivity<List<OrderItemDto>, bool>
{
    private readonly IProductService _productService;

    public CompensateProductStockUpdateActivity(IProductService productService)
    {
        _productService = productService;
    }

    public override async Task<bool> RunAsync(WorkflowActivityContext context, List<OrderItemDto> orderItems)
    {
        Console.WriteLine("[CompensateProductStockUpdateActivity] Compensation triggered.");

        if (orderItems == null || !orderItems.Any())
        {
            Console.WriteLine("[CompensateProductStockUpdateActivity] No order items to compensate.");
            return true;
        }

        Console.WriteLine("[CompensateProductStockUpdateActivity] Restoring stock for items:");

        var updates = orderItems.Select(i =>
        {
            Console.WriteLine(
                $"[CompensateProductStockUpdateActivity] - ProductId: {i.ProductId}, Restoring Qty: {i.Quantity}"
            );

            return new UpdateProductStockDto
            {
                ProductId = i.ProductId,
                QuantityChange = Math.Abs(i.Quantity) // always add stock back
            };
        }).ToList();

        Console.WriteLine("[CompensateProductStockUpdateActivity] Calling ProductService.UpdateProductStockAsync...");

        var result = await _productService.UpdateProductStockAsync(updates);

        if (!result.IsSuccess)
        {
            var errorMessage = result.Errors.FirstOrDefault()?.Message ?? "Failed to restore stock";

            Console.WriteLine($"[CompensateProductStockUpdateActivity] FAILED restoring stock: {errorMessage}");

            throw new InvalidOperationException(errorMessage);
        }

        Console.WriteLine("[CompensateProductStockUpdateActivity] Stock successfully restored.");

        return true;
    }

}
