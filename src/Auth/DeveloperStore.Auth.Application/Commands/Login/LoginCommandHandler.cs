using ErrorOr;
using MediatR;
using DeveloperStore.Auth.Application.External;
using DeveloperStore.Auth.Application.Security;
using DeveloperStore.Auth.Contracts.Security;

namespace DeveloperStore.Auth.Application.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, ErrorOr<string>>
{
    private readonly IUsersApiClient _usersApiClient;
    private readonly IJwtProvider _jwtProvider;

    public LoginCommandHandler(IUsersApiClient usersApiClient, IJwtProvider jwtProvider)
    {
        _usersApiClient = usersApiClient;
        _jwtProvider = jwtProvider;
    }

    public async Task<ErrorOr<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _usersApiClient.GetByUsernameAsync(request.Username);

        if (user is null || user.Password != request.Password)
        {
            return Error.Unauthorized(
                code: "Auth.InvalidCredentials",
                description: "Invalid username or password.");
        }
        
        var payload = new TokenPayload
        {
            UserId = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role
        };

        var token = _jwtProvider.Generate(payload);

        return token;
    }
}