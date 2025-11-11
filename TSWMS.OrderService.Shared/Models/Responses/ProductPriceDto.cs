namespace TSWMS.OrderService.Shared.Models.Responses;

public class ProductPriceDto
{
    public Guid ProductId { get; set; }
    public decimal UnitPrice { get; set; }
}