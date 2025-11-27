#region Usings

using AutoMapper;
using Dapr.Workflow;
using Microsoft.AspNetCore.Mvc;
using TSWMS.OrderService.Api.Dto;
using TSWMS.OrderService.Shared.Interfaces;

#endregion

namespace TSWMS.OrderService.Api.Controllers;

[Route("api/orders")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly DaprWorkflowClient _workflowClient;

    private readonly IOrderManager _orderManager;
    private readonly IMapper _mapper;

    public OrderController(DaprWorkflowClient workflowClient, IOrderManager orderManager, IMapper mapper)
    {
        _workflowClient = workflowClient;
        _orderManager = orderManager;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        var orders = await _orderManager.GetOrdersAsync();

        if (orders == null || !orders.Any())
        {
            return NotFound("No orders found.");
        }

        return Ok(_mapper.Map<List<OrderDto>>(orders));
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto, CancellationToken cancellationToken)
    {
        Console.WriteLine("[OrderService: Controller] Received CreateOrder request.");

        if (createOrderDto == null || createOrderDto.OrderItems == null || !createOrderDto.OrderItems.Any())
        {
            Console.WriteLine("[OrderService: Controller] Invalid request: missing order items.");
            return BadRequest("Order must have at least one item.");
        }
        if (createOrderDto.OrderItems.Any(i => i.Quantity <= 0))
        {
            throw new ArgumentException("Quantity must be positive");
        }

        Console.WriteLine($"[OrderService: Controller] Order contains {createOrderDto.OrderItems.Count} items.");

        var workflowInstanceId = Guid.NewGuid().ToString();

        Console.WriteLine($"[OrderService: Controller] Scheduling workflow instance {workflowInstanceId}...");

        await _workflowClient.ScheduleNewWorkflowAsync(
            name: nameof(CreateOrderWorkflow),
            instanceId: workflowInstanceId,
            input: createOrderDto
        );

        Console.WriteLine($"[OrderService: Controller] Workflow scheduled. Waiting for completion...");

        WorkflowState workflowState;
        try
        {
            workflowState = await _workflowClient.WaitForWorkflowCompletionAsync(
                workflowInstanceId,
                getInputsAndOutputs: true,
                cancellation: cancellationToken
            );

            Console.WriteLine($"[OrderService: Controller] Workflow {workflowInstanceId} finished with status: {workflowState.RuntimeStatus}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[OrderService: Controller] ERROR while waiting for workflow: {ex.Message}");

            return StatusCode(500, new
            {
                workflowInstanceId,
                error = ex.Message
            });
        }

        // Interpret result
        switch (workflowState.RuntimeStatus)
        {
            case WorkflowRuntimeStatus.Completed:
                Console.WriteLine($"[OrderService: Controller] Workflow {workflowInstanceId} COMPLETED successfully.");
                return Ok(workflowState.ReadOutputAs<OrderDto>());

            case WorkflowRuntimeStatus.Failed:
                Console.WriteLine($"[OrderService: Controller] Workflow FAILED: {workflowState.FailureDetails?.ErrorMessage}");
                return StatusCode(500, new
                {
                    workflowInstanceId,
                    error = workflowState.FailureDetails?.ErrorMessage ?? "Workflow failed"
                });

            case WorkflowRuntimeStatus.Terminated:
                Console.WriteLine($"[OrderService: Controller] Workflow {workflowInstanceId} TERMINATED.");
                return StatusCode(500, new
                {
                    workflowInstanceId,
                    error = "Workflow was terminated"
                });

            default:
                Console.WriteLine($"[OrderService: Controller] Workflow ended in unexpected state.");
                return Accepted(new { workflowInstanceId });
        }
    }

    //[HttpPost]
    //public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto orderDto)
    //{
    //    if (orderDto == null || orderDto.OrderItems == null || !orderDto.OrderItems.Any())
    //    {
    //        return BadRequest("Order must have at least one item.");
    //    }

    //    var workflowInstanceId = Guid.NewGuid().ToString();

    //    await _workflowClient.ScheduleNewWorkflowAsync(
    //        name: nameof(CreateOrderWorkflow),
    //        instanceId: workflowInstanceId,
    //        input: orderDto);

    //    return Accepted(new { workflowInstanceId });
    //}

    //[HttpPost]
    //public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto orderDto)
    //{
    //    var order = _mapper.Map<Order>(orderDto);

    //    var result = await _orderManager.CreateOrderAsync(order);
    //    if (result.IsFailed)
    //    {
    //        return BadRequest(result.Errors.First().Message);
    //    }

    //    return Ok(result.Value);
    //}

}