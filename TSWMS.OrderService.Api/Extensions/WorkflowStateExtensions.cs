using Dapr.Workflow;
using TSWMS.OrderService.Api.Dto;
using TSWMS.OrderService.Shared.Models.DTOs;

namespace TSWMS.OrderService.Api.Extensions;

public static class WorkflowStateExtensions
{
    public static WorkflowStatusResponse ToApiResponse(this WorkflowState state, string instanceId)
    {
        // 1. Centralized "Safe Error" Logic
        string? clientError = null;

        switch (state.RuntimeStatus)
        {
            case WorkflowRuntimeStatus.Failed:
                clientError = "Order creation failed due to an internal error. Please try again later.";
                break;
            case WorkflowRuntimeStatus.Terminated:
                clientError = "The order processing was cancelled by the system.";
                break;
            case WorkflowRuntimeStatus.Suspended:
                clientError = "Order processing is currently paused.";
                break;
            default:
                clientError = null;
                break;
        }

        // 2. Map and Return
        return new WorkflowStatusResponse
        {
            WorkflowInstanceId = instanceId,
            RuntimeStatus = state.RuntimeStatus.ToString(),
            CreatedAt = state.CreatedAt,
            LastUpdatedAt = state.LastUpdatedAt,
            Output = state.RuntimeStatus == WorkflowRuntimeStatus.Completed
                     ? state.ReadOutputAs<OrderDto>()
                     : null,
            Error = clientError
        };
    }
}
