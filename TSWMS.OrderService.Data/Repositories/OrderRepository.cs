#region Usings

using Microsoft.EntityFrameworkCore;
using TSWMS.OrderService.Shared.Interfaces;
using TSWMS.OrderService.Shared.Models;

#endregion

namespace TSWMS.OrderService.Data.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrdersDbContext _orderDbContext;

    public OrderRepository(OrdersDbContext orderDbContext)
    {
        _orderDbContext = orderDbContext;
    }

    public async Task<IEnumerable<Order>> GetOrders()
    {
        return await _orderDbContext.Orders
            .Include(order => order.OrderItems)
            .ToListAsync();
    }

    public async Task<Order> CreateOrder(Order order)
    {
        if (order == null)
        {
            throw new ArgumentNullException(nameof(order), "Order cannot be null.");
        }

        await _orderDbContext.Orders.AddAsync(order);

        await _orderDbContext.SaveChangesAsync();

        return order;
    }

    public decimal GetProductPrice(Guid productId)
    {
        throw new NotImplementedException();
    }
}
