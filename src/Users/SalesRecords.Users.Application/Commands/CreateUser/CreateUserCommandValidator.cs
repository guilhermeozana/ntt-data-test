using FluentValidation;
using SalesRecords.Users.Domain.Enums;

namespace SalesRecords.Users.Application.Commands.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone number is required.");

        RuleFor(x => x.Status)
            .Must(status => Enum.TryParse<UserStatus>(status, true, out _))
            .WithMessage("Invalid status value.");

        RuleFor(x => x.Role)
            .Must(role => Enum.TryParse<UserRole>(role, true, out _))
            .WithMessage("Invalid role value.");

        RuleFor(x => x.Name.Firstname)
            .NotEmpty().WithMessage("First name is required.");

        RuleFor(x => x.Name.Lastname)
            .NotEmpty().WithMessage("Last name is required.");

        RuleFor(x => x.Address.City)
            .NotEmpty().WithMessage("City is required.");

        RuleFor(x => x.Address.Street)
            .NotEmpty().WithMessage("Street is required.");

        RuleFor(x => x.Address.Number)
            .GreaterThan(0).WithMessage("Address number must be greater than zero.");

        RuleFor(x => x.Address.Zipcode)
            .NotEmpty().WithMessage("Zip code is required.");

        RuleFor(x => x.Address.Geolocation.Lat)
            .NotEmpty().WithMessage("Latitude is required.");

        RuleFor(x => x.Address.Geolocation.Long)
            .NotEmpty().WithMessage("Longitude is required.");
    }
}