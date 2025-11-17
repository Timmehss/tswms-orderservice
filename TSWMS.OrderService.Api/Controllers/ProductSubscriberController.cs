using Dapr;
using Microsoft.AspNetCore.Mvc;
using TSWMS.OrderService.Shared.Interfaces;
using TSWMS.OrderService.Shared.Models.Events;

namespace TSWMS.OrderService.Api.Controllers;

public class ProductSubscriberController : ControllerBase
{
    private readonly IProductEventHandler _productEventHandler;

    public ProductSubscriberController(IProductEventHandler productEventHandler)
    {
        _productEventHandler = productEventHandler;
    }

    [Topic("pubsub", "product.updated")]
    [HttpPost("product-updated")]
    public async Task<IActionResult> ReceiveProductUpdatedEvent([FromBody] ProductUpdatedEvent @event)
    {
        // Basic payload validation
        if (@event == null)
        {
            return BadRequest("Event payload is null.");
        }
        if (@event.ProductId == null)
        {
            return BadRequest("Event contains no valid product id.");
        }

        // Delegate all business logic to the business layer
        await _productEventHandler.HandleProductUpdatedEventAsync(@event);

        return Ok();
    }

}