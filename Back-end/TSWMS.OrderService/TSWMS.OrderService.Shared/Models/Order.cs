namespace TSWMS.OrderService.Shared.Models;

public class Order
{
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
    public List<OrderItem> OrderItems { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime OrderDate { get; set; }
}
