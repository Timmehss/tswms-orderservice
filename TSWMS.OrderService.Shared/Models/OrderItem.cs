namespace TSWMS.OrderService.Shared.Models;

public class OrderItem
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }

    private int _quantity;
    public int Quantity
    {
        get => _quantity;
        set
        {
            _quantity = value;
            UpdateTotalPrice();
        }
    }

    private decimal _unitPrice;
    public decimal UnitPrice
    {
        get => _unitPrice;
        set
        {
            _unitPrice = value;
            UpdateTotalPrice();
        }
    }

    public decimal TotalPrice { get; private set; }

    public Order Order { get; set; }

    private void UpdateTotalPrice()
    {
        TotalPrice = _quantity * _unitPrice;
    }
}