using Dapr.Workflow;
using TSWMS.OrderService.Shared.Interfaces;
using TSWMS.OrderService.Shared.Models.DTOs;

namespace TSWMS.OrderService.Business.Workflows.Activities;

public class CompensateCreateOrderActivity : WorkflowActivity<OrderDto, bool>
{
    private readonly IOrderManager _orderManager;

    public CompensateCreateOrderActivity(IOrderManager orderManager)
    {
        _orderManager = orderManager;
    }

    public override async Task<bool> RunAsync(WorkflowActivityContext context, OrderDto orderDto)
    {
        Console.WriteLine("[CompensateCreateOrderActivity] Compensation triggered.");

        if (orderDto == null)
        {
            Console.WriteLine("[CompensateCreateOrderActivity] No order to compensate (orderDto was null).");
            return true;
        }

        Console.WriteLine($"[CompensateCreateOrderActivity] Deleting order {orderDto.OrderId}...");

        await _orderManager.DeleteOrderAsync(orderDto.OrderId);

        Console.WriteLine($"[CompensateCreateOrderActivity] Order {orderDto.OrderId} successfully deleted.");

        return true;
    }

}
