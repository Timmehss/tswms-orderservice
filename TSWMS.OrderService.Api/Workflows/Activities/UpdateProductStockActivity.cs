using Dapr.Workflow;
using TSWMS.OrderService.Api.Dto;
using TSWMS.OrderService.Shared.Interfaces;
using TSWMS.OrderService.Shared.Models.DTOs;

namespace TSWMS.OrderService.Api.Workflows.Activities;

public class UpdateProductStockActivity
    : WorkflowActivity<List<CreateOrderItemDto>, bool>
{
    private readonly IProductService _productService;

    public UpdateProductStockActivity(IProductService productService)
    {
        _productService = productService;
    }

    public override async Task<bool> RunAsync(
           WorkflowActivityContext context,
           List<CreateOrderItemDto> orderItems)
    {
        Console.WriteLine("[UpdateProductStockActivity] Received request to update stock for:");
        foreach (var item in orderItems)
        {
            Console.WriteLine($"[UpdateProductStockActivity] - ProductId: {item.ProductId}, Quantity: {item.Quantity}");
        }

        var updates = orderItems.Select(i => new UpdateProductStockDto
        {
            ProductId = i.ProductId,
            QuantityChange = -i.Quantity
        }).ToList();

        Console.WriteLine("[UpdateProductStockActivity] Calling ProductService.UpdateProductStockAsync...");
        var result = await _productService.UpdateProductStockAsync(updates);

        if (!result.IsSuccess)
        {
            var errorMessage = result.Errors.FirstOrDefault()?.Message ?? "Failed to update product stock";
            Console.WriteLine($"[UpdateProductStockActivity] FAILED: {errorMessage}");
            throw new InvalidOperationException(errorMessage);
        }

        Console.WriteLine("[UpdateProductStockActivity] Stock update successful.");
        return true;
    }

}