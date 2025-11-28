using Dapr.Workflow;
using TSWMS.OrderService.Api.Dto;
using TSWMS.OrderService.Shared.Interfaces;
using TSWMS.OrderService.Shared.Models.DTOs;

namespace TSWMS.OrderService.Api.Workflows.Activities;

public class DeductProductStockActivity : WorkflowActivity<List<CreateOrderItemDto>, bool>
{
    private readonly IProductService _productService;

    public DeductProductStockActivity(IProductService productService)
    {
        _productService = productService;
    }

    public override async Task<bool> RunAsync(
        WorkflowActivityContext context,
        List<CreateOrderItemDto> orderItems)
    {
        Console.WriteLine("[DeductProductStockActivity] Deducting stock for items:");

        var updates = orderItems.Select(i =>
        {
            Console.WriteLine($"[DeductProductStockActivity] - ProductId: {i.ProductId}, Qty: {i.Quantity}");
            return new UpdateProductStockDto
            {
                ProductId = i.ProductId,
                QuantityChange = i.Quantity
            };
        }).ToList();

        var result = await _productService.DeductStockAsync(updates);

        if (!result.IsSuccess)
        {
            var errorMessage = result.Errors.FirstOrDefault()?.Message ?? "Failed to deduct stock";
            Console.WriteLine($"[DeductProductStockActivity] FAILED: {errorMessage}");
            throw new InvalidOperationException(errorMessage);
        }

        Console.WriteLine("[DeductProductStockActivity] Stock successfully deducted.");
        return true;
    }
}