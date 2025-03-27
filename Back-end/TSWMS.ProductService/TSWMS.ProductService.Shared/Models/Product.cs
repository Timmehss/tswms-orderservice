namespace TSWMS.ProductService.Shared.Models;

public class Product
{
    public Guid ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int AvailableStock { get; set; }
}
