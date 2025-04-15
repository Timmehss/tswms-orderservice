namespace TSWMS.OrderService.Shared.Models.Responses;

public class BatchProductPriceResponse
{
    public List<ProductPrice> ProductPrices { get; set; } = new();
}
