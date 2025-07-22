using System.Net.Http.Json;
using SalesRecords.Auth.Application.External;
using SalesRecords.Shared.SharedKernel.Pagination;
using SalesRecords.Users.Contracts.Dtos;

namespace SalesRecords.Auth.Infrastructure.External.Users;

public class UsersApiClient : IUsersApiClient
{
    private readonly HttpClient _httpClient;

    public UsersApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<UserDto> GetByUsernameAsync(string username)
    {
        var response = await _httpClient.GetAsync($"/users?filters=username:{username}");
        if (!response.IsSuccessStatusCode) return null;

        var pagedResult = await response.Content.ReadFromJsonAsync<PagedResult<UserDto>>();
        if (pagedResult == null || pagedResult.Items == null)
            return null;

        return pagedResult.Items.FirstOrDefault();
    }
}