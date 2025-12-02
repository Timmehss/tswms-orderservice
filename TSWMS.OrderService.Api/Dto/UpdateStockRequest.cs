using TSWMS.OrderService.Shared.Models.DTOs;

namespace TSWMS.OrderService.Shared.Models.Requests;

public class UpdateStockRequest
{
    public Guid OrderId { get; set; }
    public List<CreateOrderItemDto> OrderItems { get; set; }
}
