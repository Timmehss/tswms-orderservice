using TSWMS.OrderService.Shared.Models.Requests;
using TSWMS.OrderService.Shared.Models.Responses;

namespace TSWMS.OrderService.Shared.Interfaces;

public interface IProductPriceRequester
{
    Task InitializeAsync();
    Task<BatchProductPriceResponse> RequestProductPricesAsync(BatchProductPriceRequest request);
}

