using SalesRecords.Auth.Contracts.Security;

namespace SalesRecords.Auth.Application.Security;

public interface IJwtProvider
{
    string Generate(TokenPayload payload);
}