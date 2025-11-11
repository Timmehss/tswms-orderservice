using FluentResults;
using TSWMS.OrderService.Shared.Interfaces;
using TSWMS.OrderService.Shared.Interfaces.Clients;
using TSWMS.OrderService.Shared.Interfaces.Publishers;
using TSWMS.OrderService.Shared.Models;
using TSWMS.OrderService.Shared.Models.DTOs;
using TSWMS.OrderService.Shared.Models.Events;

namespace TSWMS.OrderService.Business.Managers;

public class OrderManager : IOrderManager
{
    private readonly IEventPublisher _eventPublisher;
    private readonly IOrderRepository _orderRepository;
    private readonly IProductClient _productClient;

    public OrderManager(IEventPublisher eventPublisher, IOrderRepository orderRepository, IProductClient productClient)
    {
        _eventPublisher = eventPublisher;
        _orderRepository = orderRepository;
        _productClient = productClient;
    }

    public async Task<IEnumerable<Order>> GetOrdersAsync()
    {
        return await _orderRepository.GetOrders();
    }

    public async Task<Result<Order>> CreateOrderAsync(Order order)
    {
        if (order == null || !order.OrderItems.Any())
        {
            return Result.Fail("Order must have at least one item.");
        }

        var productIds = order.OrderItems
            .Select(item => item.ProductId)
            .Distinct()
            .ToList();

        // Fetch product prices via Dapr direct service invocation
        var productPrices = await _productClient.GetProductPricesAsync(productIds);
        if (productPrices == null || !productPrices.Any())
        {
            return Result.Fail("Failed to retrieve product prices.");
        }

        // Assign prices to the products in the order and calculate total
        foreach (var item in order.OrderItems)
        {
            var product = productPrices.FirstOrDefault(p => p.ProductId == item.ProductId);
            if (product == null)
            {
                return Result.Fail($"No price found for product {item.ProductId}");
            }

            item.UnitPrice = product.UnitPrice;
            order.TotalPrice += product.UnitPrice * item.Quantity;
        }

        // Set the order date
        order.OrderDate = DateTime.UtcNow;

        // Create the order
        var createdOrder = await _orderRepository.CreateOrder(order);

        // Check if the order creation was successful
        if (createdOrder == null)
        {
            return Result.Fail("Error creating the order.");
        }

        var orderCreatedEvent = new OrderCreatedEvent
        {
            OrderId = createdOrder.OrderId,
            OrderItems = createdOrder.OrderItems
                .Select(orderItem => new OrderItemEventDto
                {
                    ProductId = orderItem.ProductId,
                    Quantity = orderItem.Quantity
                })
                .ToList()
        };

        await _eventPublisher.PublishAsync(orderCreatedEvent);

        return createdOrder;
    }

}