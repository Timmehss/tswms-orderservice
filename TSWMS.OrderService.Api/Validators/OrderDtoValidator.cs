using FluentValidation;
using TSWMS.OrderService.Api.Dto;

public class OrderDtoValidator : AbstractValidator<OrderDto>
{
    public OrderDtoValidator()
    {
        RuleFor(order => order.UserId).NotEmpty().WithMessage("User ID cannot be empty.");
        RuleFor(order => order.OrderItems).NotEmpty().WithMessage("Order must have at least one item.");
        RuleFor(order => order.TotalAmount).GreaterThan(0).WithMessage("Total amount must be greater than zero.");
        RuleFor(order => order.OrderDate).LessThanOrEqualTo(DateTime.Now).WithMessage("Order date cannot be in the future.");

        RuleForEach(order => order.OrderItems).SetValidator(new OrderItemDtoValidator());
    }
}

public class OrderItemDtoValidator : AbstractValidator<OrderItemDto>
{
    public OrderItemDtoValidator()
    {
        RuleFor(item => item.ProductId).NotEmpty().WithMessage("Product ID cannot be empty.");
        RuleFor(item => item.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than zero.");
        RuleFor(item => item.Price).GreaterThan(0).WithMessage("Price must be greater than zero.");
    }
}