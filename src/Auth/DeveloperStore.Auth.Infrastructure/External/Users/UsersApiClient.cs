using System.Net.Http.Json;
using DeveloperStore.Auth.Application.External;
using DeveloperStore.Shared.Contracts.Dtos;
using DeveloperStore.Shared.SharedKernel.Pagination;

namespace DeveloperStore.Auth.Infrastructure.External.Users;

public class UsersApiClient : IUsersApiClient
{
    private readonly HttpClient _httpClient;

    public UsersApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<UserDto> GetByUsernameAsync(string username)
    {
        var response = await _httpClient.GetAsync($"users/byUsername/{username}");
        if (!response.IsSuccessStatusCode) return null;

        var userDto = await response.Content.ReadFromJsonAsync<UserDto>();
        if (userDto == null)
            return null;

        return userDto;
    }
}