using MediatR;
using ErrorOr;

namespace SalesRecords.Auth.Application.Commands.Login;

public class LoginCommand : IRequest<ErrorOr<string>>
{
    public string Username { get; set; }
    public string Password { get; set; }
}