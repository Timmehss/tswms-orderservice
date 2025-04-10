using TSWMS.OrderService.Shared.Interfaces;
using TSWMS.OrderService.Shared.Models;

namespace TSWMS.OrderService.Business.Managers;

public class OrderManager : IOrderManager
{
    private readonly IOrderRepository _orderRepository;

    public OrderManager(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<IEnumerable<Order>> GetOrders()
    {
        return await _orderRepository.GetOrders();
    }

    public async Task CreateOrder(Order order)
    {
        return await _orderRepository.CreateOrder(order);
    }
}
