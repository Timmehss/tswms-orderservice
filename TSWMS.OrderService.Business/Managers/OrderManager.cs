using TSWMS.OrderService.Shared.Interfaces;
using TSWMS.OrderService.Shared.Models;
using TSWMS.OrderService.Shared.Models.Requests;

namespace TSWMS.OrderService.Business.Managers;

public class OrderManager : IOrderManager
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductPriceRequester _productPriceRequester;

    public OrderManager(IOrderRepository orderRepository, IProductPriceRequester productPriceRequester)
    {
        _orderRepository = orderRepository;
        _productPriceRequester = productPriceRequester;
    }

    public async Task<IEnumerable<Order>> GetOrdersAsync()
    {
        return await _orderRepository.GetOrders();
    }

    public async Task<Order> CreateOrderAsync(Order order)
    {
        if (order == null || !order.OrderItems.Any())
            throw new ArgumentException("Order must have at least one item.");

        var productIds = order.OrderItems
            .Select(item => item.ProductId)
            .Distinct()
            .ToList();

        var request = new BatchProductPriceRequest { ProductIds = productIds };
        var response = await _productPriceRequester.RequestProductPricesAsync(request);

        foreach (var item in order.OrderItems)
        {
            var product = response.ProductPrices.FirstOrDefault(p => p.ProductId == item.ProductId);
            if (product == null)
                throw new InvalidOperationException($"No price found for product {item.ProductId}");

            item.UnitPrice = product.UnitPrice;
            order.TotalPrice += product.UnitPrice * item.Quantity;
        }

        order.OrderDate = DateTime.UtcNow;

        var createdOrder = await _orderRepository.CreateOrder(order);

        return createdOrder;
    }

}
