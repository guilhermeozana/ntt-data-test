using FluentValidation;
using SalesRecords.Products.Application.Commands.CreateProduct;

namespace SalesRecords.Products.Application.Commands.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.");
        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Price).GreaterThan(0);
    }
}