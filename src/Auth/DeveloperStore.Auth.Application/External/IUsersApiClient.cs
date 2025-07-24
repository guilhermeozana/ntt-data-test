using DeveloperStore.Shared.Contracts.Dtos;

namespace DeveloperStore.Auth.Application.External;

public interface IUsersApiClient
{
    Task<UserDto> GetByUsernameAsync(string username);
}