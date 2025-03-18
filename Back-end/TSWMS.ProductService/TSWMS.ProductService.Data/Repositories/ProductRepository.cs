using Microsoft.EntityFrameworkCore;
using TSWMS.ProductService.Shared.Interfaces;
using TSWMS.ProductService.Shared.Models;

namespace TSWMS.ProductService.Data.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ProductsDbContext _productDbContext;

    public ProductRepository(ProductsDbContext productDbContext)
    {
        _productDbContext = productDbContext;
    }

    public async Task<IEnumerable<Product>> GetProducts()
    {
        return await _productDbContext.Products.ToListAsync();
    }
}
