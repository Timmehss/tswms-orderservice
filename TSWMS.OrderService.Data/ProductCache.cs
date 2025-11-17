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

    private string Key(Guid productId, string property) => $"product:{productId}:{property}";
    private string KeysListKey(Guid productId) => $"product:{productId}:keys";

    public async Task<List<ProductPriceDto>> GetProductPricesAsync(IEnumerable<Guid> productIds)
    {
        var keys = productIds.Select(id => Key(id, "price")).ToList();
        Console.WriteLine($"[ProductCache] Getting cached prices for keys: {string.Join(", ", keys)}");

        var stateItems = await _stateStore.GetBulkAsync<decimal?>(keys);
        Console.WriteLine($"[ProductCache] Retrieved {stateItems.Count} items from state store.");

        var results = new List<ProductPriceDto>();
        var missingIds = new List<Guid>();

        foreach (var kvp in stateItems)
        {
            var id = Guid.Parse(kvp.Key.Split(':')[1]);
            var value = kvp.Value;

            if (value == null)
            {
                Console.WriteLine($"[ProductCache] Cache miss for product {id}");
                missingIds.Add(id);
                continue;
            }

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

    public async Task SetPriceAsync(Guid productId, decimal price)
    {
        // Save the price
        await _stateStore.SaveAsync(Key(productId, "price"), price, TTL_SECONDS);

        // Update the keys list for this product
        var keys = await _stateStore.GetAsync<List<string>>(KeysListKey(productId)) ?? new List<string>();
        if (!keys.Contains("price"))
            keys.Add("price");

        await _stateStore.SaveAsync(KeysListKey(productId), keys, TTL_SECONDS);

        Console.WriteLine($"[ProductCache] Price cached and keys list updated for product {productId}");
    }

    public async Task DeleteProductCacheAsync(Guid productId)
    {
        // Get all keys for this product
        var keys = await _stateStore.GetAsync<List<string>>(KeysListKey(productId)) ?? new List<string>();

        foreach (var property in keys)
        {
            await _stateStore.DeleteAsync(Key(productId, property));
            Console.WriteLine($"[ProductCache] Deleted cache key: {Key(productId, property)}");
        }

        // Delete the keys list itself
        await _stateStore.DeleteAsync(KeysListKey(productId));
        Console.WriteLine($"[ProductCache] Deleted keys list for product {productId}");
    }
}