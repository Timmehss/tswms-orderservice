using Dapr.Client;
using Microsoft.Extensions.Configuration;
using TSWMS.OrderService.Shared.Interfaces.Clients;
using TSWMS.OrderService.Shared.Models.Responses;

namespace TSWMS.OrderService.Data.Clients;

public class ProductClient : IProductClient
{
    private readonly DaprClient _daprClient;

    private readonly string _productServiceAppId;
    private readonly string _getProductPricesEndpoint;

    public ProductClient(DaprClient daprClient, IConfiguration config)
    {
        _daprClient = daprClient;

        // Load configuration values
        _productServiceAppId = config["ProductService:AppId"];
        _getProductPricesEndpoint = config["ProductService:Endpoints:GetProductPrices"];
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

}