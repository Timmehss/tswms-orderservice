using TSWMS.OrderService.Shared.Models;

namespace TSWMS.OrderService.Shared.Testing;

public static class TestDataHelper
{
    public static List<Order> GetFakeOrders()
    {
        return new List<Order>
        {
            new Order
            {
                OrderId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                TotalAmount = 150.00M,
                OrderDate = DateTime.Now,
                OrderItems = CreateFakeOrderItems()
            },
            new Order
            {
                OrderId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                TotalAmount = 200.00M,
                OrderDate = DateTime.Now,
                OrderItems = CreateFakeOrderItems()
            }
        };
    }

    private static List<OrderItem> CreateFakeOrderItems()
    {
        return new List<OrderItem>
        {
            new OrderItem
            {
                OrderId = Guid.NewGuid(),
                ProductId = Guid.NewGuid(),
                Quantity = 2,
                Price = 25.00m
            },
            new OrderItem
            {
                OrderId = Guid.NewGuid(),
                ProductId = Guid.NewGuid(),
                Quantity = 1,
                Price = 50.00m
            },
            new OrderItem
            {
                OrderId = Guid.NewGuid(),
                ProductId = Guid.NewGuid(),
                Quantity = 3,
                Price = 10.00m
            }
        };
    }
}