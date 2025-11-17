using Dapr.Client;
using Microsoft.Extensions.Configuration;
using TSWMS.OrderService.Shared.Interfaces;

namespace TSWMS.OrderService.Data;

public class RedisStateStore : IStateStore
{
    private readonly DaprClient _dapr;
    private readonly string _storeName;

    private int parallelism = 10;

    public RedisStateStore(DaprClient dapr, IConfiguration config)
    {
        _dapr = dapr;
        _storeName = config["Dapr:OrderServiceStateStoreName"]!;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        return await _dapr.GetStateAsync<T>(_storeName, key);
    }

    public async Task<Dictionary<string, T?>> GetBulkAsync<T>(IEnumerable<string> keys)
    {
        var keyList = keys.ToList();
        Console.WriteLine($"[RedisStateStore] Getting bulk state for keys: {string.Join(", ", keyList)}");

        if (keyList.Count == 0)
        {
            Console.WriteLine("[RedisStateStore] No keys provided, returning empty dictionary.");
            return new Dictionary<string, T?>();
        }

        var items = await _dapr.GetBulkStateAsync<T>(
            _storeName,
            keyList,
            parallelism,
            metadata: null
        );

        Console.WriteLine($"[RedisStateStore] Retrieved {items.Count} items from state store.");

        foreach (var item in items)
        {
            Console.WriteLine($"[RedisStateStore] Key {item.Key} => Value: {item.Value}");
        }

        return items.ToDictionary(i => i.Key, i => i.Value);
    }

    public async Task SaveAsync<T>(string key, T value, int? ttlSeconds = null)
    {
        Console.WriteLine($"[RedisStateStore] Saving key {key} with value {value} and TTL {ttlSeconds}");

        var metadata = ttlSeconds.HasValue
            ? new Dictionary<string, string> { ["ttlInSeconds"] = ttlSeconds.Value.ToString() }
            : null;

        await _dapr.SaveStateAsync(_storeName, key, value, metadata: metadata);

        Console.WriteLine($"[RedisStateStore] Key {key} saved successfully.");
    }

    public async Task DeleteAsync(string key)
    {
        await _dapr.DeleteStateAsync(_storeName, key);
        Console.WriteLine($"[RedisStateStore] Key {key} deleted successfully.");
    }

}