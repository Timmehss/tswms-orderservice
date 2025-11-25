namespace TSWMS.OrderService.Api.Workflows;

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
        // Collect productIds from DTO
        var productIds = createOrderDto.OrderItems
            .Select(i => i.ProductId)
            .Distinct()
            .ToList();

        // Step 1: Fetch prices
        var prices = await context.CallActivityAsync<List<ProductPriceDto>>(
            nameof(GetProductPricesActivity),
            productIds);

        // Step 2: Create order
        var result = await context.CallActivityAsync<OrderDto>(
            nameof(CreateOrderActivity),
            new CreateOrderActivityDto
            {
                CreateOrderDto = createOrderDto,
                Prices = prices
            });

        return result;
    }
}