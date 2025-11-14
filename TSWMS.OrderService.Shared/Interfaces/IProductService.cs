using TSWMS.OrderService.Shared.Models.Responses;

namespace TSWMS.OrderService.Shared.Interfaces;

public interface IProductService
{
    Task<List<ProductPriceDto>> GetProductPricesAsync(List<Guid> productIds);
}