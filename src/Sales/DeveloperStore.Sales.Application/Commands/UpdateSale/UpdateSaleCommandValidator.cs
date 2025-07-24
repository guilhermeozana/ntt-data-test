using FluentValidation;

namespace DeveloperStore.Sales.Application.Commands.UpdateSale;

public class UpdateSaleCommandValidator : AbstractValidator<UpdateSaleCommand>
{
    public UpdateSaleCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Sale ID is required.");

        RuleFor(x => x.CustomerId)
            .GreaterThan(0).WithMessage("Customer ID is required.");

        RuleFor(x => x.BranchId)
            .GreaterThan(0).WithMessage("Branch ID is required.");

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Date is required.")
            .Must(BeAValidDate).WithMessage("Date must be in a valid format.");

        RuleFor(x => x.Products)
            .NotEmpty().WithMessage("At least one product is required.");

        RuleForEach(x => x.Products).ChildRules(product =>
        {
            product.RuleFor(p => p.ProductId)
                .GreaterThan(0).WithMessage("Product ID is required.");

            product.RuleFor(p => p.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.")
                .LessThanOrEqualTo(20).WithMessage("Cannot purchase more than 20 identical items.");

            product.RuleFor(p => p.UnitPrice)
                .GreaterThan(0).WithMessage("Unit price must be greater than zero.");
        });
    }

    private bool BeAValidDate(string dateString)
    {
        return DateTime.TryParse(dateString, out _);
    }
}