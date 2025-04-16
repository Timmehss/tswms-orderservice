#region Usings

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using TSWMS.OrderService.Shared.Interfaces;
using TSWMS.OrderService.Shared.Models.Requests;
using TSWMS.OrderService.Shared.Models.Responses;

#endregion

namespace TSWMS.OrderService.Data;

public class ProductPriceRequester : IProductPriceRequester
{
    private readonly IConnectionFactory _connectionFactory;
    private IConnection? _connection;
    private IChannel? _channel;

    public ProductPriceRequester(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task InitializeAsync()
    {
        _connection = await _connectionFactory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        // Optionally declare the queue to ensure it exists
        await _channel.QueueDeclareAsync(
            queue: "product.price.request",
            durable: true,
            exclusive: false,
            autoDelete: false
        );
    }

    public async Task<BatchProductPriceResponse> RequestProductPricesAsync(BatchProductPriceRequest request)
    {
        if (_channel == null)
            throw new InvalidOperationException("RabbitMqProductPriceRequester is not initialized. Call InitializeAsync() before using.");

        var tcs = new TaskCompletionSource<BatchProductPriceResponse>();  // Task to await reply

        var correlationId = Guid.NewGuid().ToString();

        // Declare a temporary, exclusive reply queue
        var replyQueue = await _channel.QueueDeclareAsync(
            queue: string.Empty, // Let RabbitMQ generate a random queue name
            durable: false,
            exclusive: true,
            autoDelete: true
        );

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var responseCorrelationId = ea.BasicProperties?.CorrelationId;

            if (responseCorrelationId == correlationId)
            {
                var json = Encoding.UTF8.GetString(body);
                var response = JsonSerializer.Deserialize<BatchProductPriceResponse>(json);
                tcs.SetResult(response!);
            }

            await Task.Yield(); // let the event complete
        };

        // Start consuming the reply queue
        await _channel.BasicConsumeAsync(
            consumer: consumer,
            queue: replyQueue.QueueName,
            autoAck: true
        );

        var props = new BasicProperties
        {
            CorrelationId = correlationId,
            ReplyTo = replyQueue.QueueName,
        };

        var messageBody = JsonSerializer.SerializeToUtf8Bytes(request);

        // Publish to product.price.request queue
        await _channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: "product.price.request",
            mandatory: true,
            basicProperties: props,
            body: messageBody
        );

        // Timeout logic: fail if no reply after 10 seconds
        var completedTask = await Task.WhenAny(tcs.Task, Task.Delay(TimeSpan.FromSeconds(60)));

        if (completedTask != tcs.Task)
            throw new TimeoutException("Timed out waiting for product price response from ProductService.");

        return await tcs.Task;
    }

}

