using AutoMapper;
using Dapr.Workflow;
using TSWMS.OrderService.Shared.Interfaces;
using TSWMS.OrderService.Shared.Models;
using TSWMS.OrderService.Shared.Models.DTOs;

namespace TSWMS.OrderService.Business.Workflows.Activities;

public class CreateOrderActivity : WorkflowActivity<CreateOrderActivityDto, OrderDto>
{
    private readonly IOrderManager _orderManager;
    private readonly IMapper _mapper;

    public CreateOrderActivity(IOrderManager orderManager, IMapper mapper)
    {
        _orderManager = orderManager;
        _mapper = mapper;
    }

    public override async Task<OrderDto> RunAsync(WorkflowActivityContext context, CreateOrderActivityDto createOrderActivityDto)
    {
        // Map the DTO to the Domain Entity
        var order = _mapper.Map<Order>(createOrderActivityDto.OrderDetails);

        // Pass BOTH the order entity AND the prices we fetched earlier
        var result = await _orderManager.CreateOrderAsync(order, createOrderActivityDto.LockedPrices);

        if (result.IsFailed)
        {
            // Throwing exception triggers Dapr Workflow retry/failure logic
            throw new InvalidOperationException(result.Errors.First().Message);
        }

        return _mapper.Map<OrderDto>(result.Value);
    }

}
