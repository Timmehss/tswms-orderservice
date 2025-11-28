using Dapr.Client;
using FluentResults;
using Microsoft.Extensions.Configuration;
using TSWMS.OrderService.Shared.Interfaces.Clients;
using TSWMS.OrderService.Shared.Models.DTOs;
using TSWMS.OrderService.Shared.Models.Responses;
using TSWMS.OrderService.Shared.Results;

namespace TSWMS.OrderService.Data.Clients;

public class ProductClient : IProductClient
{
    private readonly DaprClient _daprClient;

    private readonly string _productServiceAppId;
    private readonly string _getProductPricesEndpoint;
    private readonly string _updateProductStockAsync;
    private readonly string _deductProductStockAsync;
    private readonly string _restoreProductStockAsync;

    public ProductClient(DaprClient daprClient, IConfiguration config)
    {
        _daprClient = daprClient;

        // Load configuration values
        _productServiceAppId = config["ProductService:AppId"];
        _getProductPricesEndpoint = config["ProductService:Endpoints:GetProductPrices"];
        _updateProductStockAsync = config["ProductService:Endpoints:UpdateProductStockAsync"];
        _deductProductStockAsync = config["ProductService:Endpoints:DeductProductStockAsync"];
        _restoreProductStockAsync = config["ProductService:Endpoints:RestoreProductStockAsync"];
    }

    public async Task<Result> DeductStockAsync(List<UpdateProductStockDto> updates)
    {
        try
        {
            var response = await _daprClient.InvokeMethodAsync<List<UpdateProductStockDto>, StockUpdateResultDto>(
                HttpMethod.Put,
                _productServiceAppId,
                _deductProductStockAsync,
                updates);

            // 3. Mapping
            if (response.Success) return Result.Ok();
            return Result.Fail(response.ErrorMessage ?? "Unknown stock deduction error");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ProductClient] Exception invoking Dapr: {ex.Message}");
            return Result.Fail($"System Failure: {ex.Message}");
        }
    }

    public async Task<Result> RestoreStockAsync(List<UpdateProductStockDto> updates)
    {
        try
        {
            var response = await _daprClient.InvokeMethodAsync<List<UpdateProductStockDto>, StockUpdateResultDto>(
                HttpMethod.Put,
                _productServiceAppId,
                _restoreProductStockAsync,
                updates);

            if (response.Success) return Result.Ok();
            return Result.Fail(response.ErrorMessage ?? "Unknown stock restoration error");
        }
        catch (Exception ex)
        {
            return Result.Fail($"System Failure: {ex.Message}");
        }
    }

    public async Task<List<ProductPriceDto>> GetProductPricesAsync(List<Guid> productIds)
    {
        // Build query string for GET request
        var queryString = string.Join("&", productIds.Select(id => $"productIds={id}"));
        var endpoint = $"{_getProductPricesEndpoint}?{queryString}";

        var products = await _daprClient.InvokeMethodAsync<List<ProductPriceDto>>(
            HttpMethod.Get,
            _productServiceAppId,
            endpoint
        );

        return products;
    }

    public async Task<Result> UpdateProductAvailableStockAsync(List<UpdateProductStockDto> updates)
    {
        Console.WriteLine("[ProductClient] Sending stock update request to ProductService.");
        Console.WriteLine($"[ProductClient] PUT -> AppId: {_productServiceAppId}, Endpoint: {_updateProductStockAsync}");
        Console.WriteLine($"[ProductClient] Updating {updates.Count} products.");

        var result = await _daprClient.InvokeMethodAsync<List<UpdateProductStockDto>, bool>(
            HttpMethod.Put,
            _productServiceAppId,
            _updateProductStockAsync,
            updates);

        Console.WriteLine($"[ProductClient] Result received. IsSuccess={result}");

        return Result.Ok();
    }

}