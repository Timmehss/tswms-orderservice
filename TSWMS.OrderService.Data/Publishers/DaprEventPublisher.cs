using Dapr.Client;
using Microsoft.Extensions.Configuration;
using TSWMS.OrderService.Shared.Interfaces.Publishers;
using TSWMS.OrderService.Shared.Models.Events;

namespace TSWMS.OrderService.Data.Publishers;

public class DaprEventPublisher : IEventPublisher
{
    private readonly DaprClient _daprClient;

    private readonly string _pubSubName;
    private readonly Dictionary<Type, string> _eventTopics;

    public DaprEventPublisher(DaprClient daprClient, IConfiguration config)
    {
        _daprClient = daprClient;
        _pubSubName = config["Dapr:ComponentNames:PubSub"];

        _eventTopics = new Dictionary<Type, string>
        {
            { typeof(OrderCreatedEvent), config["Dapr:Topics:Orders:OrderCreated"] }
        };
    }

    public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : class
    {
        var type = typeof(TEvent);
        if (!_eventTopics.TryGetValue(type, out var topicName))
        {
            throw new InvalidOperationException($"No topic configured for event {type.Name}");
        }

        await _daprClient.PublishEventAsync(_pubSubName, topicName, @event);
    }

}