using TSWMS.OrderService.Shared.Models;

namespace TSWMS.OrderService.Shared.Interfaces;

public interface IOrderManager
{
    Task<IEnumerable<Order>> GetOrdersAsync();
    Task<Order> CreateOrderAsync(Order order);
}
