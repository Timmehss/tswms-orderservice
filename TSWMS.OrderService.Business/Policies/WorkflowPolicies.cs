using Dapr.Workflow;

namespace TSWMS.OrderService.Business.Policies;

public static class WorkflowPolicies
{
    // A standard policy for most database or external API calls
    public static WorkflowRetryPolicy StandardRetryPolicy => new WorkflowRetryPolicy(
        firstRetryInterval: TimeSpan.FromSeconds(2),
        backoffCoefficient: 2.0,
        maxRetryInterval: TimeSpan.FromMinutes(1),
        maxNumberOfAttempts: 3);

    // A more aggressive policy for critical, fast operations
    public static WorkflowRetryPolicy AggressiveRetryPolicy => new WorkflowRetryPolicy(
        firstRetryInterval: TimeSpan.FromMilliseconds(500),
        backoffCoefficient: 1.5,
        maxRetryInterval: TimeSpan.FromSeconds(5),
        maxNumberOfAttempts: 5);

    // A policy for long-running processes
    public static WorkflowRetryPolicy LongWaitRetryPolicy => new WorkflowRetryPolicy(
        firstRetryInterval: TimeSpan.FromSeconds(10),
        backoffCoefficient: 1.0,
        maxRetryInterval: TimeSpan.FromSeconds(30),
        maxNumberOfAttempts: 10);
}