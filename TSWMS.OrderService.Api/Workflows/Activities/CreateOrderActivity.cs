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
        // Map DTO -> domain
        var order = _mapper.Map<Order>(createOrderDto);

        // Call business logic
        var result = await _orderManager.CreateOrderAsync(order);
        if (!result.IsSuccess)
        {
            throw new InvalidOperationException(result.Errors.First().Message ?? "Failed to create order");
        }

        // Map domain -> DTO
        return _mapper.Map<OrderDto>(result.Value);
    }
}
