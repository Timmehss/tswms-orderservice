using TSWMS.OrderService.Shared.Models.Responses;

namespace TSWMS.OrderService.Shared.Interfaces.Clients;

public interface IProductClient
{
    Task<List<ProductPriceDto>> GetProductPricesAsync(List<Guid> productIds);
}