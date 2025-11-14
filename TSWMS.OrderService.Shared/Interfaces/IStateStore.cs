namespace TSWMS.OrderService.Shared.Interfaces;

public interface IStateStore
{
    Task<T?> GetAsync<T>(string key);
    Task<Dictionary<string, T?>> GetBulkAsync<T>(IEnumerable<string> keys);
    Task SaveAsync<T>(string key, T value, int? ttlSeconds = null);
    Task DeleteAsync(string key);
}