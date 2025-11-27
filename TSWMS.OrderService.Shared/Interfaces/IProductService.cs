using FluentResults;
using TSWMS.OrderService.Shared.Models.DTOs;
using TSWMS.OrderService.Shared.Models.Responses;

namespace TSWMS.OrderService.Shared.Interfaces;

public interface IProductService
{
    Task<Result> UpdateProductStockAsync(List<UpdateProductStockDto> items);
    Task<List<ProductPriceDto>> GetProductPricesAsync(List<Guid> productIds);
}