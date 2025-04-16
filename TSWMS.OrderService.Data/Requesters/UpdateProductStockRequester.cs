using RabbitMQ.Client;
using System.Text.Json;
using TSWMS.OrderService.Shared.Interfaces;
using TSWMS.OrderService.Shared.Models.Requests;

namespace TSWMS.OrderService.Data.Requesters;

public class UpdateProductStockRequester : IUpdateProductStockRequester
{
    private readonly IConnectionFactory _connectionFactory;
    private IConnection? _connection;
    private IChannel? _channel;

    public UpdateProductStockRequester(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task InitializeAsync()
    {
        _connection = await _connectionFactory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        // Optionally declare the queue to ensure it exists
        await _channel.QueueDeclareAsync(
            queue: "product.stock.update",
            durable: true,
            exclusive: false,
            autoDelete: false
        );
    }

    public async Task SendStockUpdateRequestAsync(IEnumerable<UpdateProductStock> stockUpdates)
    {
        if (_channel == null)
            throw new InvalidOperationException("UpdateStockRequester is not initialized. Call InitializeAsync() before using.");

        var request = new UpdateProductStockRequest { UpdateProductStocks = stockUpdates.ToList() };
        var messageBody = JsonSerializer.SerializeToUtf8Bytes(request);

        var props = new BasicProperties
        {
            CorrelationId = Guid.NewGuid().ToString(),
        };

        // Publish to product.stock.update queue
        await _channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: "product.stock.update",
            mandatory: true,
            basicProperties: props,
            body: messageBody
        );
    }
}
