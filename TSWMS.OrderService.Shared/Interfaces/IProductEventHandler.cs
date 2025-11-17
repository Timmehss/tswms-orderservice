using TSWMS.OrderService.Shared.Models.Events;

namespace TSWMS.OrderService.Shared.Interfaces;

public interface IProductEventHandler
{
    Task HandleProductUpdatedEventAsync(ProductUpdatedEvent @event);
}