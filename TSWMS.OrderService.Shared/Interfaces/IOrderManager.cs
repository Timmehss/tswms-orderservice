using FluentResults;
using TSWMS.OrderService.Shared.Models;
using TSWMS.OrderService.Shared.Models.Responses;

namespace TSWMS.OrderService.Shared.Interfaces;

public interface IOrderManager
{
    Task<IEnumerable<Order>> GetOrdersAsync();
    //Task<Result<Order>> CreateOrderAsync(Order order);
    Task DeleteOrderAsync(Guid orderId);
    Task<Result<Order>> CreateOrderAsync(Order order, List<ProductPriceDto> productPrices);
}