using Dapr.Client;
using FluentResults;
using Microsoft.Extensions.Configuration;
using TSWMS.OrderService.Shared.Interfaces;
using TSWMS.OrderService.Shared.Interfaces.Clients;
using TSWMS.OrderService.Shared.Models;
using TSWMS.OrderService.Shared.Models.DTOs;
using TSWMS.OrderService.Shared.Models.Events;

namespace TSWMS.OrderService.Business.Managers;

public class OrderManager : IOrderManager
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductClient _productClient;

    private readonly DaprClient _daprClient;

    private readonly string _pubSubName;
    private readonly string _orderCreatedTopic;

    public OrderManager(DaprClient daprClient, IOrderRepository orderRepository, IProductClient productClient, IConfiguration config)
    {
        _daprClient = daprClient;

        _orderRepository = orderRepository;
        _productClient = productClient;

        _pubSubName = config["Dapr:ComponentNames:PubSub"] ?? "";
        _orderCreatedTopic = config["Dapr:Topics:Orders:OrderCreated"] ?? "";
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

        await _daprClient.PublishEventAsync(_pubSubName, _orderCreatedTopic, orderCreatedEvent);

        return createdOrder;
    }

}