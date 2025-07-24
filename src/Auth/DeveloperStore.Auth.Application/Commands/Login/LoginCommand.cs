using MediatR;
using ErrorOr;

namespace DeveloperStore.Auth.Application.Commands.Login;

public class LoginCommand : IRequest<ErrorOr<string>>
{
    public LoginCommand(string username, string password)
    {
        Username = username;
        Password = password;
    }

    public string Username { get; set; }
    public string Password { get; set; }
}