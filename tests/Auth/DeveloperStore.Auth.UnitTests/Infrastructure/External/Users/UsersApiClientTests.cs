using System.Net;
using System.Net.Http.Json;
using DeveloperStore.Auth.Infrastructure.External.Users;
using DeveloperStore.Shared.Contracts.Dtos;
using FluentAssertions;
using RichardSzalay.MockHttp;

namespace DeveloperStore.Auth.UnitTests.Infrastructure.External.Users;

public class UsersApiClientTests
{
    [Fact]
    public async Task GetByUsernameAsync_ShouldReturnUser_WhenResponseIsSuccessful()
    {
        // Arrange
        var username = "guilherme";
        var expectedUser = new UserDto
        {
            Id = 1,
            Username = username,
            Email = "gui@example.com",
            Role = "Customer",
            Status = "Active",
            Phone = "11999999999",
            Name = new NameDto { Firstname = "Guilherme", Lastname = "Dev" },
            Address = new AddressDto { Street = "Rua XPTO", Number = 42, City = "RJ" }
        };

        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When($"http://localhost/users/byUsername/{username}")
                .Respond(HttpStatusCode.OK, JsonContent.Create(expectedUser));

        var httpClient = mockHttp.ToHttpClient();
        httpClient.BaseAddress = new Uri("http://localhost"); // ✅ ESSENCIAL

        var client = new UsersApiClient(httpClient);

        // Act
        var result = await client.GetByUsernameAsync(username);

        // Assert
        result.Should().BeEquivalentTo(expectedUser);
    }

    [Fact]
    public async Task GetByUsernameAsync_ShouldReturnNull_WhenResponseIsNotSuccessful()
    {
        var username = "notfound";

        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When($"http://localhost/users/byUsername/{username}")
                .Respond(HttpStatusCode.NotFound);

        var httpClient = mockHttp.ToHttpClient();
        httpClient.BaseAddress = new Uri("http://localhost");

        var client = new UsersApiClient(httpClient);

        var result = await client.GetByUsernameAsync(username);

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByUsernameAsync_ShouldReturnNull_WhenContentIsEmpty()
    {
        var username = "guilherme";

        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When($"http://localhost/users/byUsername/{username}")
            .Respond(req => new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(string.Empty, System.Text.Encoding.UTF8, "application/json")
            });

        var httpClient = mockHttp.ToHttpClient();
        httpClient.BaseAddress = new Uri("http://localhost");

        var client = new UsersApiClient(httpClient);

        var result = await client.GetByUsernameAsync(username);

        result.Should().BeNull();
    }
}