using TSWMS.OrderService.Shared.Models.Requests;

namespace TSWMS.OrderService.Shared.Interfaces;

public interface IUpdateProductStockRequester
{
    Task InitializeAsync();
    Task SendStockUpdateRequestAsync(IEnumerable<UpdateProductStock> stockUpdates);
}
