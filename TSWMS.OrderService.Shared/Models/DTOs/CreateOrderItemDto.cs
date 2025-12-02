namespace TSWMS.OrderService.Shared.Models.DTOs;

public class CreateOrderItemDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
