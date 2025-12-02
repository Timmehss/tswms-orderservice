using TSWMS.OrderService.Shared.Models.Responses;

namespace TSWMS.OrderService.Shared.Models.DTOs;

public class CreateOrderActivityDto
{
    public CreateOrderDto OrderDetails { get; set; } = null!;
    public List<ProductPriceDto> LockedPrices { get; set; } = new();
}
