using TSWMS.OrderService.Shared.Models.DTOs;

namespace TSWMS.OrderService.Shared.Models.Events;

public class OrderCreatedEvent
{
    public Guid OrderId { get; set; }
    public List<OrderItemEventDto> OrderItems { get; set; } = new();
}