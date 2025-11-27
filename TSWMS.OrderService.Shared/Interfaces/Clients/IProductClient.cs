using FluentResults;
using TSWMS.OrderService.Shared.Models.DTOs;
using TSWMS.OrderService.Shared.Models.Responses;

namespace TSWMS.OrderService.Shared.Interfaces.Clients;

public interface IProductClient
{
    Task<List<ProductPriceDto>> GetProductPricesAsync(List<Guid> productIds);
    Task<Result> UpdateProductAvailableStockAsync(List<UpdateProductStockDto> updates);
}