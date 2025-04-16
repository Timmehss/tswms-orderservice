using TSWMS.OrderService.Shared.Interfaces;
using TSWMS.OrderService.Shared.Models;
using TSWMS.OrderService.Shared.Models.Requests;

namespace TSWMS.OrderService.Business.Managers;

public class OrderManager : IOrderManager
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductPriceRequester _productPriceRequester;
    private readonly IUpdateProductStockRequester _updateStockRequester;

    public OrderManager(IOrderRepository orderRepository, IProductPriceRequester productPriceRequester, IUpdateProductStockRequester updateStockRequester)
    {
        _orderRepository = orderRepository;
        _productPriceRequester = productPriceRequester;
        _updateStockRequester = updateStockRequester;
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

        if (createdOrder == null)
        {
            throw new InvalidOperationException($"There was an error creating the order!");
        }

        // Request stock update on ordered products
        var stockUpdates = order.OrderItems.Select(item => new UpdateProductStock
        {
            ProductId = item.ProductId,
            QuantityOrdered = item.Quantity
        }).ToList();

        await _updateStockRequester.SendStockUpdateRequestAsync(stockUpdates);

        return createdOrder;
    }

}
