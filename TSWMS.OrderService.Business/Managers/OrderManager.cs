using TSWMS.OrderService.Shared.Interfaces;
using TSWMS.OrderService.Shared.Models;
using TSWMS.OrderService.Shared.Models.Events;

namespace TSWMS.OrderService.Business.Managers;

public class OrderManager : IOrderManager
{
    private readonly IOrderRepository _orderRepository;
    private readonly IRabbitMqPublisher _rabbitMqPublisher;

    public OrderManager(IOrderRepository orderRepository, IRabbitMqPublisher rabbitMqPublisher)
    {
        _orderRepository = orderRepository;
        _rabbitMqPublisher = rabbitMqPublisher;
    }

    public async Task<IEnumerable<Order>> GetOrders()
    {
        return await _orderRepository.GetOrders();
    }

    public async Task<Order> CreateOrder(Order order)
    {
        // Create a new OrderId
        order.OrderId = Guid.NewGuid();

        // Set the OrderDate to the current time
        order.OrderDate = DateTime.UtcNow;

        // Calculate the total amount while setting prices for order items
        decimal totalAmount = 0;
        foreach (var item in order.OrderItems)
        {
            // Assume there's a method GetProductPrice that fetches current price from a product repository
            //item.UnitPrice = await _orderRepository.GetProductPrice(item.ProductId);
            //totalAmount += item.UnitPrice * item.Quantity;
        }

        order.TotalPrice = totalAmount;

        // Save the order to the database
        var createdOrder = await _orderRepository.CreateOrder(order);

        // Publish stock update event if order creation is successful
        if (createdOrder != null && createdOrder.OrderItems.Any())
        {
            var stockUpdateEvents = createdOrder.OrderItems.Select(item => new StockUpdateEvent
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity
            });

            await _rabbitMqPublisher.PublishStockUpdate(stockUpdateEvents);
        }

        return createdOrder;
    }
}
