//#region Usings

//using Microsoft.Extensions.Options;
//using RabbitMQ.Client;
//using RabbitMQ.Client.Events;
//using System.Text;
//using System.Text.Json;
//using TSWMS.OrderService.Shared.Helpers;
//using TSWMS.OrderService.Shared.Interfaces;
//using TSWMS.OrderService.Shared.Models.Requests;
//using TSWMS.OrderService.Shared.Models.Responses;
//using TSWMS.OrderService.Shared.Options;

//#endregion

//namespace TSWMS.OrderService.Data.Requesters;

//public class ProductPriceRequester : IProductPriceRequester
//{
//    private readonly IConnectionFactory _connectionFactory;
//    private IConnection? _connection;
//    private IChannel? _channel;
//    private readonly string _secretKey;

//    public ProductPriceRequester(IConnectionFactory connectionFactory, IOptions<HmacOptions> hmacOptions)
//    {
//        _connectionFactory = connectionFactory;
//        _secretKey = hmacOptions.Value.SecretKey;
//    }

//    public async Task InitializeAsync()
//    {
//        _connection = await _connectionFactory.CreateConnectionAsync();
//        _channel = await _connection.CreateChannelAsync();

//        // Declare the queue to ensure it exists
//        await _channel.QueueDeclareAsync(
//            queue: "product.price.request",
//            durable: true,
//            exclusive: false,
//            autoDelete: false
//        );
//    }

//    public async Task<BatchProductPriceResponse> RequestProductPricesAsync(BatchProductPriceRequest request)
//    {
//        if (_channel == null)
//            throw new InvalidOperationException("Channel in ProductPriceRequester is not initialized. Call InitializeAsync() before using.");

//        var tcs = new TaskCompletionSource<BatchProductPriceResponse>();
//        var correlationId = Guid.NewGuid().ToString();

//        // Declare a temporary, exclusive reply queue
//        var replyQueue = await _channel.QueueDeclareAsync(
//            queue: string.Empty,
//            durable: false,
//            exclusive: true,
//            autoDelete: true
//        );

//        var consumer = new AsyncEventingBasicConsumer(_channel);

//        consumer.ReceivedAsync += async (model, ea) =>
//        {
//            var body = ea.Body.ToArray();
//            var responseCorrelationId = ea.BasicProperties?.CorrelationId;

//            if (responseCorrelationId == correlationId)
//            {
//                var receivedSignature = ea.BasicProperties?.Headers != null &&
//                                        ea.BasicProperties.Headers.TryGetValue("X-Signature", out var headerValue)
//                                            ? Encoding.UTF8.GetString((byte[])headerValue)
//                                            : null;

//                if (string.IsNullOrEmpty(receivedSignature) ||
//                    !HmacHelper.ValidateHmac(body, receivedSignature, _secretKey))
//                {
//                    tcs.SetException(new InvalidOperationException("Invalid HMAC signature on response. Possible tampering detected!"));
//                }
//                else
//                {
//                    var json = Encoding.UTF8.GetString(body);
//                    var response = JsonSerializer.Deserialize<BatchProductPriceResponse>(json);
//                    tcs.SetResult(response!);
//                }
//            }

//            await Task.Yield();
//        };

//        await _channel.BasicConsumeAsync(
//            consumer: consumer,
//            queue: replyQueue.QueueName,
//            autoAck: true
//        );

//        var messageBody = JsonSerializer.SerializeToUtf8Bytes(request);

//        // Generate HMAC signature for the outgoing message
//        var signature = HmacHelper.GenerateHmac(messageBody, _secretKey);

//        var props = new BasicProperties
//        {
//            CorrelationId = correlationId,
//            ReplyTo = replyQueue.QueueName,
//            Headers = new Dictionary<string, object>
//            {
//                { "X-Signature", signature }
//            }
//        };

//        await _channel.BasicPublishAsync(
//            exchange: string.Empty,
//            routingKey: "product.price.request",
//            mandatory: true,
//            basicProperties: props,
//            body: messageBody
//        );

//        // Timeout logic
//        var completedTask = await Task.WhenAny(tcs.Task, Task.Delay(TimeSpan.FromSeconds(15)));

//        if (completedTask != tcs.Task)
//            throw new TimeoutException("Timed out waiting for product price response from ProductService.");

//        return await tcs.Task;
//    }
//}
