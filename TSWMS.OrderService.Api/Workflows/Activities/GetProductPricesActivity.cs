using Dapr.Workflow;
using TSWMS.OrderService.Shared.Interfaces;
using TSWMS.OrderService.Shared.Models.Responses;

namespace TSWMS.OrderService.Api.Workflows.Activities;

public class GetProductPricesActivity : WorkflowActivity<List<Guid>, List<ProductPriceDto>>
{
    private readonly IProductService _productService;

    public GetProductPricesActivity(IProductService productService)
    {
        _productService = productService;
    }

    public override async Task<List<ProductPriceDto>> RunAsync(
        WorkflowActivityContext context,
        List<Guid> productIds)
    {
        return await _productService.GetProductPricesAsync(productIds);
    }

}
