#region Usings

using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using TSWMS.OrderService.Shared.Interfaces;
using TSWMS.OrderService.Shared.Models.Events;

#endregion

namespace TSWMS.OrderService.Data;

public class RabbitMqPublisher : IRabbitMqPublisher
{
    private IConnection _connection;
    private IChannel _channel;

    public RabbitMqPublisher()
    {

    }

    public async Task InitAsync()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };

        _connection = await factory.CreateConnectionAsync();

        _channel = await _connection.CreateChannelAsync();

        await _channel.QueueDeclareAsync(queue: "stock_update_queue",
                              durable: true,
                              exclusive: false,
                              autoDelete: false,
                              arguments: null);
    }

    public async Task PublishStockUpdate(IEnumerable<StockUpdateEvent> stockUpdates)
    {
        var message = JsonSerializer.Serialize(stockUpdates);
        var body = Encoding.UTF8.GetBytes(message);

        await _channel.BasicPublishAsync(exchange: string.Empty,
                              routingKey: "stock_update_queue",
                              body: body);
    }
}
