using AutoMapper;
using Dapr.Workflow;
using TSWMS.OrderService.Api.Dto;
using TSWMS.OrderService.Shared.Interfaces;
using TSWMS.OrderService.Shared.Models;

namespace TSWMS.OrderService.Api.Workflows.Activities;

public class CreateOrderActivity : WorkflowActivity<CreateOrderDto, OrderDto>
{
    private readonly IOrderManager _orderManager;
    private readonly IMapper _mapper;

    public CreateOrderActivity(IOrderManager orderManager, IMapper mapper)
    {
        _orderManager = orderManager;
        _mapper = mapper;
    }

    public override async Task<OrderDto> RunAsync(
        WorkflowActivityContext context,
        CreateOrderDto createOrderDto)
    {
        var order = _mapper.Map<Order>(createOrderDto);

        Console.WriteLine("[CreateOrderActivity] Calling OrderManager.CreateOrderAsync...");
        var result = await _orderManager.CreateOrderAsync(order);
        if (!result.IsSuccess)
        {
            Console.WriteLine($"[CreateOrderActivity] FAILED: {result.Errors.First().Message}");
            throw new InvalidOperationException(result.Errors.First().Message ?? "Failed to create order");
        }

        Console.WriteLine($"[CreateOrderActivity] Successfully created order {result.Value.OrderId}");

        var dto = _mapper.Map<OrderDto>(result.Value);

        Console.WriteLine($"[CreateOrderActivity] Returning OrderDto for order {dto.OrderId}");
        return dto;
    }

}
