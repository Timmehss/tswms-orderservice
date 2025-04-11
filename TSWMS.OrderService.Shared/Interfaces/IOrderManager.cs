using TSWMS.OrderService.Shared.Models;

namespace TSWMS.OrderService.Shared.Interfaces;

public interface IOrderManager
{
    Task<IEnumerable<Order>> GetOrders();
    Task<Order> CreateOrder(Order order);
}
