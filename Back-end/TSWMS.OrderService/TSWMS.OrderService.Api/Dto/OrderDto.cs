namespace TSWMS.OrderService.Api.Dto;

public class OrderDto
{
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
    public List<OrderItemDto> OrderItems { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime OrderDate { get; set; }
}
