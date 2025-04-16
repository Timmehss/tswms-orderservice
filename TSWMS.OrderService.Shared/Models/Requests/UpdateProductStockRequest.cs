namespace TSWMS.OrderService.Shared.Models.Requests;

public class UpdateProductStockRequest
{
    public List<UpdateProductStock> UpdateProductStocks { get; set; } = new List<UpdateProductStock>();
}

public class UpdateProductStock
{
    public Guid ProductId { get; set; }
    public int QuantityOrdered { get; set; }
}
