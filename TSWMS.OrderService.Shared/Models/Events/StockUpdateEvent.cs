namespace TSWMS.OrderService.Shared.Models.Events;

public class StockUpdateEvent
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
