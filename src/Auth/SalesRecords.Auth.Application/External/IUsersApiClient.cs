using SalesRecords.Users.Contracts.Dtos;

namespace SalesRecords.Auth.Application.External;

public interface IUsersApiClient
{
    Task<UserDto> GetByUsernameAsync(string username);
}