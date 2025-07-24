using FluentValidation;
using DeveloperStore.Sales.Application.Commands.CreateSale;

namespace DeveloperStore.Sales.Application.Commands.CreateSale;

public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    public CreateSaleCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .GreaterThan(0).WithMessage("Customer ID is required.");

        RuleFor(x => x.BranchId)
            .GreaterThan(0).WithMessage("Branch ID is required.");

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Date is required.")
            .Must(BeAValidDate).WithMessage("Date must be a valid format (yyyy-MM-dd).");

        RuleFor(x => x.Products)
            .NotEmpty().WithMessage("At least one product must be included in the sale.");

        RuleForEach(x => x.Products).ChildRules(product =>
        {
            product.RuleFor(p => p.ProductId)
                .GreaterThan(0).WithMessage("Product ID is required.");

            product.RuleFor(p => p.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.")
                .LessThanOrEqualTo(20).WithMessage("Cannot purchase more than 20 identical items.");

            product.RuleFor(p => p.UnitPrice)
                .GreaterThan(0).WithMessage("Unit price must be greater than zero.");

            product.RuleFor(p => p.Quantity)
                .Must(q => q >= 4 || q == 1 || q == 2 || q == 3)
                .WithMessage("Purchases below 4 items cannot have a discount.");
        });
    }

    private bool BeAValidDate(string dateString)
    {
        return DateTime.TryParse(dateString, out _);
    }
}