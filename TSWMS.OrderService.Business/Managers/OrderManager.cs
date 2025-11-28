using FluentResults;
using TSWMS.OrderService.Shared.Interfaces;
using TSWMS.OrderService.Shared.Interfaces.Clients;
using TSWMS.OrderService.Shared.Interfaces.Publishers;
using TSWMS.OrderService.Shared.Models;

namespace TSWMS.OrderService.Business.Managers;

public class OrderManager : IOrderManager
{
    private readonly IEventPublisher _eventPublisher;
    private readonly IOrderRepository _orderRepository;
    private readonly IProductClient _productClient;
    private readonly IProductService _productService;

    public OrderManager(
        IEventPublisher eventPublisher,
        IOrderRepository orderRepository,
        IProductClient productClient,
        IProductService productService
        )
    {
        _eventPublisher = eventPublisher;
        _orderRepository = orderRepository;
        _productClient = productClient;
        _productService = productService;
    }

    public async Task<IEnumerable<Order>> GetOrdersAsync()
    {
        return await _orderRepository.GetOrders();
    }

    public async Task<Result<Order>> CreateOrderAsync(Order order)
    {
        Console.WriteLine("[OrderManager] CreateOrderAsync started.");

        if (order == null || !order.OrderItems.Any())
        {
            Console.WriteLine("[OrderManager] Order invalid: must have at least one item.");
            return Result.Fail("Order must have at least one item.");
        }

        Console.WriteLine("[OrderManager] Fetching product prices...");

        var productIds = order.OrderItems
            .Select(item => item.ProductId)
            .Distinct()
            .ToList();

        var productPrices = await _productService.GetProductPricesAsync(productIds);
        if (productPrices == null || !productPrices.Any())
        {
            Console.WriteLine("[OrderManager] FAILED: Could not fetch product prices.");
            return Result.Fail("Failed to retrieve product prices.");
        }

        Console.WriteLine("[OrderManager] Assigning prices and calculating total.");

        foreach (var item in order.OrderItems)
        {
            var price = productPrices.FirstOrDefault(p => p.ProductId == item.ProductId);
            if (price == null)
            {
                Console.WriteLine($"[OrderManager] FAILED: Missing price for product {item.ProductId}");
                return Result.Fail($"Error calculating order total, price is missing for product {item.ProductId}");
            }

            item.UnitPrice = price.UnitPrice;
            order.TotalPrice += item.UnitPrice * item.Quantity;
        }

        order.OrderDate = DateTime.UtcNow;

        Console.WriteLine("[OrderManager] Saving order in repository...");

        var createdOrder = await _orderRepository.CreateOrder(order);

        if (createdOrder == null)
        {
            Console.WriteLine("[OrderManager] FAILED: Repository returned null order.");
            return Result.Fail("Error creating the order.");
        }

        Console.WriteLine($"[OrderManager] Order created successfully with ID {createdOrder.OrderId}.");

        return createdOrder;
    }

    public async Task DeleteOrderAsync(Guid orderId)
    {
        Console.WriteLine($"[OrderManager] DeleteOrderAsync called for OrderId: {orderId}");

        if (orderId == Guid.Empty)
        {
            Console.WriteLine("[OrderManager] FAILED: OrderId cannot be empty.");
            throw new ArgumentException("OrderId cannot be empty.", nameof(orderId));
        }

        Console.WriteLine("[OrderManager] Calling repository to delete order...");
        await _orderRepository.DeleteOrderAsync(orderId);

        Console.WriteLine($"[OrderManager] Order {orderId} deleted successfully.");
    }

}