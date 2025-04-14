using TSWMS.OrderService.Shared.Models.Events;

namespace TSWMS.OrderService.Shared.Interfaces;

public interface IRabbitMqPublisher
{
    Task InitAsync();
    Task PublishStockUpdate(IEnumerable<StockUpdateEvent> stockUpdates);
}
