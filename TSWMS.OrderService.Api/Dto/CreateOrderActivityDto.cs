using TSWMS.OrderService.Shared.Models.Responses;

namespace TSWMS.OrderService.Api.Dto;

public class CreateOrderActivityDto
{
    public CreateOrderDto CreateOrderDto { get; set; } = null!;
    public List<ProductPriceDto> Prices { get; set; } = new();
}
