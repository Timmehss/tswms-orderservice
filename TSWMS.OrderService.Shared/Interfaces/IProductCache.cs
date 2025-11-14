using TSWMS.OrderService.Shared.Models.Responses;

namespace TSWMS.OrderService.Shared.Interfaces;

public interface IProductCache
{
    Task<List<ProductPriceDto>> GetProductPricesAsync(IEnumerable<Guid> productIds);
    Task SetPriceAsync(Guid productId, decimal price);
}