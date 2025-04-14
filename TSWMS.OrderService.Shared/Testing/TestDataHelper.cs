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
                TotalPrice = 150.00M,
                OrderDate = DateTime.Now,
                OrderItems = CreateFakeOrderItems()
            },
            new Order
            {
                OrderId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                TotalPrice = 200.00M,
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
                UnitPrice = 25.00m
            },
            new OrderItem
            {
                OrderId = Guid.NewGuid(),
                ProductId = Guid.NewGuid(),
                Quantity = 1,
                UnitPrice = 50.00m
            },
            new OrderItem
            {
                OrderId = Guid.NewGuid(),
                ProductId = Guid.NewGuid(),
                Quantity = 3,
                UnitPrice = 10.00m
            }
        };
    }
}