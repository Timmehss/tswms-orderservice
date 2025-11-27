namespace TSWMS.OrderService.Shared.Models.DTOs;

public class UpdateProductStockDto
{
    public Guid ProductId { get; set; }
    public int QuantityChange { get; set; }
}
