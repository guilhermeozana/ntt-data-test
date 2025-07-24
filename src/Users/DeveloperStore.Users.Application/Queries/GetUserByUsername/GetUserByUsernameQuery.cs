using DeveloperStore.Shared.Contracts.Dtos;
using ErrorOr;
using MediatR;

namespace DeveloperStore.Users.Application.Queries.GetUserByUsername;

public class GetUserByUsernameQuery : IRequest<ErrorOr<UserDto>>
{
    public string Username { get; }

    public GetUserByUsernameQuery(string username)
    {
        Username = username;
    }
}