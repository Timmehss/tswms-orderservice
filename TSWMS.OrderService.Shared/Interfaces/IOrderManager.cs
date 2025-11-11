using FluentResults;
using TSWMS.OrderService.Shared.Models;

namespace TSWMS.OrderService.Shared.Interfaces;

public interface IOrderManager
{
    Task<IEnumerable<Order>> GetOrdersAsync();
    Task<Result<Order>> CreateOrderAsync(Order order);
}