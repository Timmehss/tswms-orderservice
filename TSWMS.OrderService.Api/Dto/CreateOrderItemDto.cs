namespace TSWMS.OrderService.Api.Dto;

public class CreateOrderItemDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
