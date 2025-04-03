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
}
