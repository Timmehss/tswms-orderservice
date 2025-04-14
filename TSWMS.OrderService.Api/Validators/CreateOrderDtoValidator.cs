using FluentValidation;
using TSWMS.OrderService.Api.Dto;

public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
{
    public CreateOrderDtoValidator()
    {
        RuleFor(order => order.OrderItems).NotEmpty().WithMessage("Order must have at least one item.");

        RuleForEach(order => order.OrderItems).SetValidator(new CreateOrderItemDtoValidator());
    }
}

public class CreateOrderItemDtoValidator : AbstractValidator<CreateOrderItemDto>
{
    public CreateOrderItemDtoValidator()
    {
        RuleFor(item => item.ProductId).NotEmpty().WithMessage("Product ID cannot be empty.");
        RuleFor(item => item.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than zero.");
    }
}