using FluentResults;
using TSWMS.OrderService.Shared.Interfaces;
using TSWMS.OrderService.Shared.Interfaces.Clients;
using TSWMS.OrderService.Shared.Models.DTOs;
using TSWMS.OrderService.Shared.Models.Responses;

namespace TSWMS.OrderService.Business;

public class ProductService : IProductService
{
    private readonly IProductCache _productCache;
    private readonly IProductClient _productClient;

    public ProductService(IProductCache productCache, IProductClient productClient)
    {
        _productCache = productCache;
        _productClient = productClient;
    }

    public async Task<Result> UpdateProductStockAsync(List<UpdateProductStockDto> items)
    {
        Console.WriteLine("[ProductService] UpdateProductStockAsync called.");
        Console.WriteLine($"[ProductService] Updating stock for {items.Count} items.");

        foreach (var item in items)
        {
            Console.WriteLine($"[ProductService] - ProductId: {item.ProductId}, Quantity Ordered: {item.QuantityChange}");
        }

        var result = await _productClient.UpdateProductAvailableStockAsync(items);

        Console.WriteLine($"[ProductService] UpdateProductAvailableStockAsync returned IsSuccess={result.IsSuccess}");

        return result;
    }

    public async Task<List<ProductPriceDto>> GetProductPricesAsync(List<Guid> productIds)
    {
        Console.WriteLine($"[ProductService] Requested product IDs: {string.Join(", ", productIds)}");

        // Get cached prices
        var cachedProductPriceDtos = await _productCache.GetProductPricesAsync(productIds);
        Console.WriteLine($"[ProductService] Retrieved {cachedProductPriceDtos.Count} cached prices.");

        // Build a hash set of IDs already in cache
        var cachedIds = cachedProductPriceDtos.Select(c => c.ProductId).ToHashSet();

        // Find missing product IDs
        var missingIds = productIds
            .Where(id => !cachedIds.Contains(id))
            .ToList();
        Console.WriteLine($"[ProductService] Missing IDs to fetch: {string.Join(", ", missingIds)}");

        List<ProductPriceDto> fetchedDtos = new();

        // Fetch missing prices
        if (missingIds.Any())
        {
            Console.WriteLine($"[ProductService] Fetching missing prices from ProductClient...");
            fetchedDtos = await _productClient.GetProductPricesAsync(missingIds);
            Console.WriteLine($"[ProductService] Fetched {fetchedDtos.Count} prices from ProductClient.");

            // Save fetched results into cache
            foreach (var dto in fetchedDtos)
            {
                Console.WriteLine($"[ProductService] Caching product {dto.ProductId} => {dto.UnitPrice}");
                await _productCache.SetPriceAsync(dto.ProductId, dto.UnitPrice);
            }
        }

        // Return merged results
        var total = cachedProductPriceDtos.Count + fetchedDtos.Count;
        Console.WriteLine($"[ProductService] Returning total {total} product prices.");

        return cachedProductPriceDtos.Concat(fetchedDtos).ToList();
    }

    //public async Task<List<ProductPriceDto>> GetProductPricesAsync(List<Guid> productIds)
    //{
    //    Console.WriteLine($"[ProductService] Requested product IDs: {string.Join(", ", productIds)}");

    //    // Get cached prices
    //    var cachedProductPriceDtos = await _productCache.GetProductPricesAsync(productIds);
    //    Console.WriteLine($"[ProductService] Retrieved {cachedProductPriceDtos.Count} cached prices.");

    //    // Build a hash set of IDs already in cache
    //    var cachedIds = cachedProductPriceDtos.Select(c => c.ProductId).ToHashSet();

    //    // Find missing product IDs
    //    var missingIds = productIds
    //        .Where(id => !cachedIds.Contains(id))
    //        .ToList();
    //    Console.WriteLine($"[ProductService] Missing IDs to fetch: {string.Join(", ", missingIds)}");

    //    List<ProductPriceDto> fetchedDtos = new();

    //    // Fetch missing prices
    //    if (missingIds.Any())
    //    {
    //        Console.WriteLine($"[ProductService] Fetching missing prices from ProductClient...");
    //        fetchedDtos = await _productClient.GetProductPricesAsync(missingIds);
    //        Console.WriteLine($"[ProductService] Fetched {fetchedDtos.Count} prices from ProductClient.");

    //        // Save fetched results into cache
    //        foreach (var dto in fetchedDtos)
    //        {
    //            Console.WriteLine($"[ProductService] Caching product {dto.ProductId} => {dto.UnitPrice}");
    //            await _productCache.SetPriceAsync(dto.ProductId, dto.UnitPrice);
    //        }
    //    }

    //    // Return merged results
    //    var total = cachedProductPriceDtos.Count + fetchedDtos.Count;
    //    Console.WriteLine($"[ProductService] Returning total {total} product prices.");

    //    return cachedProductPriceDtos.Concat(fetchedDtos).ToList();
    //}

}