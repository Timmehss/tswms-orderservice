using Dapr.Workflow;
using TSWMS.OrderService.Api.Dto;
using TSWMS.OrderService.Api.Workflows.Activities;
using TSWMS.OrderService.Shared.Models.Responses;

public class CreateOrderWorkflow : Workflow<CreateOrderDto, OrderDto>
{
    public override async Task<OrderDto> RunAsync(
        WorkflowContext context,
        CreateOrderDto createOrderDto)
    {
        OrderDto? orderDto = null;
        bool stockDeducted = false;
        try
        {
            if (!context.IsReplaying)
            {
                Console.WriteLine("[CreateOrderWorkflow] Starting CreateOrderWorkflow.");
            }

            // STEP 1 — GET PRICES
            var productIds = createOrderDto.OrderItems
                .Select(i => i.ProductId)
                .Distinct()
                .ToList();

            if (!context.IsReplaying)
            {
                Console.WriteLine($"[CreateOrderWorkflow] Fetching prices for {productIds.Count} products.");
            }

            var prices = await context.CallActivityAsync<List<ProductPriceDto>>(
                nameof(GetProductPricesActivity),
                productIds);

            // STEP 2 — CREATE ORDER RECORD
            if (!context.IsReplaying)
            {
                Console.WriteLine("[CreateOrderWorkflow] Creating order.");
            }

            orderDto = await context.CallActivityAsync<OrderDto>(
                nameof(CreateOrderActivity),
                createOrderDto);

            // STEP 3 — DEDUCT STOCK
            if (!context.IsReplaying)
            {
                Console.WriteLine("[CreateOrderWorkflow] Deducting stock.");
            }

            var stockUpdateResult = await context.CallActivityAsync<bool>(
                nameof(UpdateProductStockActivity),
                createOrderDto.OrderItems);

            // Only mark as deducted if explicitly successful
            stockDeducted = stockUpdateResult;

            if (!context.IsReplaying)
            {
                Console.WriteLine("[CreateOrderWorkflow] Workflow completed successfully.");
            }

            return orderDto;
        }
        catch (Exception ex)
        {
            if (!context.IsReplaying)
            {
                Console.WriteLine($"[CreateOrderWorkflow] ERROR: {ex.Message}. Starting compensation...");
            }

            // COMPENSATION 1: ORDER
            if (orderDto != null)
            {
                if (!context.IsReplaying)
                {
                    Console.WriteLine("[CreateOrderWorkflow] Running compensate order creation.");
                }
                await context.CallActivityAsync(
                    nameof(CompensateCreateOrderActivity),
                    orderDto);
            }

            // COMPENSATION 2: STOCK (ONLY if stock was successfully deducted earlier)
            if (stockDeducted)
            {
                if (!context.IsReplaying)
                {
                    Console.WriteLine("[CreateOrderWorkflow] Running compensate stock update.");
                }
                await context.CallActivityAsync(
                    nameof(CompensateProductStockUpdateActivity),
                    createOrderDto.OrderItems);
            }

            throw;
        }
    }
}