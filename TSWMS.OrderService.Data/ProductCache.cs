using TSWMS.OrderService.Shared.Interfaces;
using TSWMS.OrderService.Shared.Models.Responses;

namespace TSWMS.OrderService.Data;

public class ProductCache : IProductCache
{
    private readonly IStateStore _stateStore;
    private const int TTL_SECONDS = 3600;

    public ProductCache(IStateStore stateStore)
    {
        _stateStore = stateStore;
    }

    private string Key(Guid productId) => $"product:{productId}:price";

    public async Task<List<ProductPriceDto>> GetProductPricesAsync(IEnumerable<Guid> productIds)
    {
        var keys = productIds.Select(Key).ToList();
        Console.WriteLine($"[ProductCache] Getting cached prices for keys: {string.Join(", ", keys)}");

        // State store returns Dictionary<string, T?> where T is decimal
        var stateItems = await _stateStore.GetBulkAsync<decimal?>(keys);
        Console.WriteLine($"[ProductCache] Retrieved {stateItems.Count} items from state store.");

        var results = new List<ProductPriceDto>();
        var missingIds = new List<Guid>();

        foreach (var kvp in stateItems)
        {
            var id = Guid.Parse(kvp.Key.Split(':')[1]);
            var value = kvp.Value;

            // Treat null or default(decimal) as missing
            if (value == null)
            {
                Console.WriteLine($"[ProductCache] Cache miss for product {id}");
                missingIds.Add(id);
                continue;
            }

            // Cache hit
            results.Add(new ProductPriceDto
            {
                ProductId = id,
                UnitPrice = value.Value
            });

            Console.WriteLine($"[ProductCache] Cache hit: Product {id} => {value}");
        }

        if (missingIds.Any())
        {
            Console.WriteLine($"[ProductCache] Missing {missingIds.Count} product prices: {string.Join(", ", missingIds)}");
        }

        Console.WriteLine($"[ProductCache] Returning {results.Count} cached product prices.");

        return results;
    }

    public Task SetPriceAsync(Guid productId, decimal price)
    {
        return _stateStore.SaveAsync(Key(productId), price, TTL_SECONDS);
    }

    public async Task DeletePriceAsync(Guid productId)
    {
        await _stateStore.DeleteAsync(Key(productId));
    }

}