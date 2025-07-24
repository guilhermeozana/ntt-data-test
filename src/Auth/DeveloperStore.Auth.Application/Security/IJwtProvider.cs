using DeveloperStore.Auth.Contracts.Security;

namespace DeveloperStore.Auth.Application.Security;

public interface IJwtProvider
{
    string Generate(TokenPayload payload);
}