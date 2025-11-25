#region Usings

using AutoMapper;
using Dapr.Workflow;
using Microsoft.AspNetCore.Mvc;
using TSWMS.OrderService.Api.Dto;
using TSWMS.OrderService.Api.Workflows;
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
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto orderDto, CancellationToken cancellationToken)
    {
        if (orderDto == null || orderDto.OrderItems == null || !orderDto.OrderItems.Any())
            return BadRequest("Order must have at least one item.");

        var workflowInstanceId = Guid.NewGuid().ToString();

        // Step 1: schedule the workflow
        await _workflowClient.ScheduleNewWorkflowAsync(
            name: nameof(CreateOrderWorkflow),
            instanceId: workflowInstanceId,
            input: orderDto
        );

        // Step 2: wait for workflow completion
        WorkflowState workflowState;
        try
        {
            workflowState = await _workflowClient.WaitForWorkflowCompletionAsync(
                workflowInstanceId,
                getInputsAndOutputs: true,
                cancellation: cancellationToken
            );
        }
        catch (Exception ex)
        {
            // Could not wait for completion (timeout, cancellation, etc.)
            return StatusCode(500, new
            {
                workflowInstanceId,
                error = ex.Message
            });
        }

        // Step 3: check the workflow status
        switch (workflowState.RuntimeStatus)
        {
            case WorkflowRuntimeStatus.Completed:
                // Deserialize the workflow output
                var orderDtoResult = workflowState.ReadOutputAs<OrderDto>();
                return Ok(orderDtoResult);

            case WorkflowRuntimeStatus.Failed:
                var errorMessage = workflowState.FailureDetails?.ErrorMessage ?? "Workflow failed";
                return StatusCode(500, new
                {
                    workflowInstanceId,
                    error = errorMessage
                });

            case WorkflowRuntimeStatus.Terminated:
                return StatusCode(500, new
                {
                    workflowInstanceId,
                    error = "Workflow was terminated"
                });

            default:
                // Should not happen because WaitForWorkflowCompletionAsync blocks until terminal state
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