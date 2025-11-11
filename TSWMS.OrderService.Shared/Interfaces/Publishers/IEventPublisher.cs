namespace TSWMS.OrderService.Shared.Interfaces.Publishers;

public interface IEventPublisher
{
    Task PublishAsync<TEvent>(TEvent @event) where TEvent : class;
}