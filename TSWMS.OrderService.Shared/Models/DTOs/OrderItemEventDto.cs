namespace TSWMS.OrderService.Shared.Models.DTOs;

public class OrderItemEventDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}