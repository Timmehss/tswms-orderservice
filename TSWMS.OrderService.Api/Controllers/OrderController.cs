using AutoMapper;
using Dapr.Workflow;
using Microsoft.AspNetCore.Mvc;
using TSWMS.OrderService.Shared.Interfaces;
using TSWMS.OrderService.Shared.Models.DTOs;

namespace TSWMS.OrderService.Api.Controllers;

[Route("api/orders")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly DaprWorkflowClient _workflowClient;
    private readonly IOrderManager _orderManager;
    private readonly IMapper _mapper;
    private readonly ILogger<OrderController> _logger;

    public OrderController(
        DaprWorkflowClient workflowClient,
        IOrderManager orderManager,
        IMapper mapper,
        ILogger<OrderController> logger)
    {
        _workflowClient = workflowClient;
        _orderManager = orderManager;
        _mapper = mapper;
        _logger = logger;
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
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto)
    {
        _logger.LogInformation("[OrderController] Received CreateOrder request.");

        // Validation
        if (createOrderDto == null || createOrderDto.OrderItems == null || !createOrderDto.OrderItems.Any())
        {
            _logger.LogWarning("[OrderController] Invalid request: missing order items.");
            return BadRequest("Order must have at least one item.");
        }

        if (createOrderDto.OrderItems.Any(i => i.Quantity <= 0))
        {
            return BadRequest("Quantity must be positive for all items.");
        }

        var workflowInstanceId = Guid.NewGuid().ToString();

        _logger.LogInformation("Scheduling workflow instance {InstanceId} for {ItemCount} items.",
            workflowInstanceId, createOrderDto.OrderItems.Count);

        // Start the workflow but DO NOT wait for it
        await _workflowClient.ScheduleNewWorkflowAsync(
            name: nameof(CreateOrderWorkflow),
            instanceId: workflowInstanceId,
            input: createOrderDto
        );

        // Build the status URL so the client knows where to check
        var statusUrl = Url.Action(nameof(GetOrderStatus), new { instanceId = workflowInstanceId });

        // Return 202 Accepted immediately
        return Accepted(statusUrl, new
        {
            WorkflowInstanceId = workflowInstanceId,
            Status = "Pending",
            CheckStatusUrl = statusUrl
        });
    }

    [HttpGet("status/{instanceId}")]
    public async Task<IActionResult> GetOrderStatus(string instanceId)
    {
        _logger.LogDebug("Checking status for workflow {InstanceId}", instanceId);

        try
        {
            var state = await _workflowClient.GetWorkflowStateAsync(instanceId, true);

            if (state == null)
            {
                return NotFound($"Workflow {instanceId} not found.");
            }

            // Map Dapr status to API response
            var response = new
            {
                WorkflowInstanceId = instanceId,
                RuntimeStatus = state.RuntimeStatus.ToString(),
                CreatedAt = state.CreatedAt,
                LastUpdatedAt = state.LastUpdatedAt,
                Output = state.RuntimeStatus == WorkflowRuntimeStatus.Completed
                         ? state.ReadOutputAs<OrderDto>()
                         : null,
                Error = state.FailureDetails?.ErrorMessage
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving workflow status for {InstanceId}", instanceId);
            return StatusCode(500, "Error retrieving workflow status.");
        }
    }

}