using ErrorOr;
using MediatR;
using SalesRecords.Users.Contracts.Dtos;

namespace SalesRecords.Users.Application.Commands.CreateUser;

public class CreateUserCommand : IRequest<ErrorOr<UserDto>>
{
    public string Email { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
    public NameDto Name { get; set; } = new();
    public AddressDto Address { get; set; } = new();
    public string Phone { get; set; } = default!;
    public string Status { get; set; } = "Active";
    public string Role { get; set; } = "Customer";
}