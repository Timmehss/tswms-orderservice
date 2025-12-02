using Dapr.Workflow;
using TSWMS.OrderService.Business.Policies;
using TSWMS.OrderService.Business.Workflows.Activities;
using TSWMS.OrderService.Shared.Models.DTOs;
using TSWMS.OrderService.Shared.Models.Responses;

public class CreateOrderWorkflow : Workflow<CreateOrderDto, OrderDto>
{
    public override async Task<OrderDto> RunAsync(
        WorkflowContext context,
        CreateOrderDto createOrderDto)
    {
        OrderDto? orderDto = null;
        bool stockDeducted = false;

        var retryOptions = new WorkflowTaskOptions
        {
            RetryPolicy = WorkflowPolicies.StandardRetryPolicy
        };

        try
        {
            if (!context.IsReplaying) Console.WriteLine("[CreateOrderWorkflow] Starting.");

            // STEP 1 — GET PRICES
            var productIds = createOrderDto.OrderItems
                .Select(i => i.ProductId)
                .Distinct()
                .ToList();

            if (!context.IsReplaying) Console.WriteLine($"[CreateOrderWorkflow] Fetching prices for {productIds.Count} items.");

            var prices = await context.CallActivityAsync<List<ProductPriceDto>>(
                nameof(GetProductPricesActivity),
                productIds,
                retryOptions);

            // STEP 2 — CREATE ORDER RECORD
            if (!context.IsReplaying) Console.WriteLine("[CreateOrderWorkflow] Creating order.");

            var createOrderInput = new CreateOrderActivityDto
            {
                OrderDetails = createOrderDto,
                LockedPrices = prices
            };

            orderDto = await context.CallActivityAsync<OrderDto>(
                nameof(CreateOrderActivity),
                createOrderInput,
                retryOptions);

            // STEP 3 — DEDUCT STOCK
            if (!context.IsReplaying) Console.WriteLine("[CreateOrderWorkflow] Deducting stock.");

            stockDeducted = await context.CallActivityAsync<bool>(
                nameof(DeductProductStockActivity),
                createOrderDto.OrderItems,
                retryOptions);

            if (!context.IsReplaying) Console.WriteLine("[CreateOrderWorkflow] Completed successfully.");

            return orderDto;
        }
        catch (Exception ex)
        {
            if (!context.IsReplaying) Console.WriteLine($"[CreateOrderWorkflow] ERROR: {ex.Message}. Starting compensation...");

            // COMPENSATIONS
            if (stockDeducted)
            {
                if (!context.IsReplaying) Console.WriteLine("[CreateOrderWorkflow] Compensating: Restoring Stock.");

                await context.CallActivityAsync(
                    nameof(RestoreProductStockActivity),
                    createOrderDto.OrderItems,
                    retryOptions);
            }

            if (orderDto != null)
            {
                if (!context.IsReplaying) Console.WriteLine("[CreateOrderWorkflow] Compensating: Deleting Order.");

                await context.CallActivityAsync(
                    nameof(CompensateCreateOrderActivity),
                    orderDto,
                    retryOptions);
            }

            throw;
        }
    }
}