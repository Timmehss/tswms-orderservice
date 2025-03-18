using TSWMS.ProductService.Shared.Models;

namespace TSWMS.ProductService.Shared.Interfaces;

public interface IProductManager
{
    Task<IEnumerable<Product>> GetProducts();
}
