namespace TSWMS.OrderService.Api.Dto;

public class CreateOrderDto
{
    public List<CreateOrderItemDto> OrderItems { get; set; } = new List<CreateOrderItemDto>();
}
