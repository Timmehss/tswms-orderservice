//#region Usings

//using Microsoft.Extensions.Options;
//using RabbitMQ.Client;
//using System.Text.Json;
//using TSWMS.OrderService.Shared.Helpers;
//using TSWMS.OrderService.Shared.Interfaces;
//using TSWMS.OrderService.Shared.Models.Requests;
//using TSWMS.OrderService.Shared.Options;

//#endregion

//namespace TSWMS.OrderService.Data.Requesters;

//public class UpdateProductStockRequester : IUpdateProductStockRequester
//{
//    private readonly IConnectionFactory _connectionFactory;
//    private IConnection? _connection;
//    private IChannel? _channel;
//    private readonly string _secretKey;

//    public UpdateProductStockRequester(IConnectionFactory connectionFactory, IOptions<HmacOptions> hmacOptions)
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
//            queue: "product.stock.update",
//            durable: true,
//            exclusive: false,
//            autoDelete: false
//        );
//    }

//    public async Task SendStockUpdateRequestAsync(IEnumerable<UpdateProductStock> stockUpdates)
//    {
//        if (_channel == null)
//            throw new InvalidOperationException("UpdateStockRequester is not initialized. Call InitializeAsync() before using.");

//        var request = new UpdateProductStockRequest { UpdateProductStocks = stockUpdates.ToList() };
//        var messageBody = JsonSerializer.SerializeToUtf8Bytes(request);

//        // Generate HMAC signature for the message
//        var signature = HmacHelper.GenerateHmac(messageBody, _secretKey);

//        var props = new BasicProperties
//        {
//            CorrelationId = Guid.NewGuid().ToString(),
//            Headers = new Dictionary<string, object>
//            {
//                { "X-Signature", signature }
//            }
//        };

//        // Publish to product.stock.update queue
//        await _channel.BasicPublishAsync(
//            exchange: string.Empty,
//            routingKey: "product.stock.update",
//            mandatory: true,
//            basicProperties: props,
//            body: messageBody
//        );
//    }
//}
