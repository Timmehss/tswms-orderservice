using TSWMS.OrderService.Shared.Interfaces;
using TSWMS.OrderService.Shared.Models.Events;

namespace TSWMS.OrderService.Business;

public class ProductEventHandler : IProductEventHandler
{
    private readonly IProductCache _productCache;

    public ProductEventHandler(IProductCache productCache)
    {
        _productCache = productCache;
    }

    public async Task HandleProductUpdatedEventAsync(ProductUpdatedEvent @event)
    {
        Console.WriteLine($"[ProductEventHandler] Product updated: {@event.ProductId}, invalidating full cache.");
        await _productCache.DeleteProductCacheAsync(@event.ProductId);
    }

}