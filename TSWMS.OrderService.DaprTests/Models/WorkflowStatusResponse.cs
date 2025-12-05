using TSWMS.OrderService.Shared.Models.DTOs;

namespace TSWMS.OrderService.DaprTests.Models;

public class WorkflowStatusResponse
{
    public string WorkflowInstanceId { get; set; } = string.Empty;
    public string RuntimeStatus { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset LastUpdatedAt { get; set; }
    public OrderDto? Output { get; set; }
    public string? Error { get; set; }
}
