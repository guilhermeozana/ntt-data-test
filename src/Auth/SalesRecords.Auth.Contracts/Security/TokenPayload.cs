namespace SalesRecords.Auth.Contracts.Security;

public class TokenPayload
{
    public int UserId { get; init; }
    public string Username { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
}