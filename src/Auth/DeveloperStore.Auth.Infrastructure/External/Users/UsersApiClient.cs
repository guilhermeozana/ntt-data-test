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
        if (!response.IsSuccessStatusCode || response.Content == null)
            return null;

        if (response.Content.Headers.ContentLength == 0)
            return null;

        try
        {
            return await response.Content.ReadFromJsonAsync<UserDto>();
        }
        catch (System.Text.Json.JsonException)
        {
            return null;
        }
    }
}