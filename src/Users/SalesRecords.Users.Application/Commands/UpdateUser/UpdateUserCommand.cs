using ErrorOr;
using MediatR;
using SalesRecords.Users.Contracts.Dtos;

namespace SalesRecords.Users.Application.Commands.UpdateUser;

public class UpdateUserCommand : IRequest<ErrorOr<UserDto>>
{
    public int Id { get; set; }
    public string Email { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
    public NameDto Name { get; set; } = new();
    public AddressDto Address { get; set; } = new();
    public string Phone { get; set; } = default!;
    public string Status { get; set; } = default!;
    public string Role { get; set; } = default!;
}