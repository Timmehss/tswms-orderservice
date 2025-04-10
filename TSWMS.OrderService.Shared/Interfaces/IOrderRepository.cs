using TSWMS.OrderService.Shared.Models;

namespace TSWMS.OrderService.Shared.Interfaces;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetOrders();
    Task CreateOrder(Order order);
}
