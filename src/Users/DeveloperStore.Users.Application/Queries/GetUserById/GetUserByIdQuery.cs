using ErrorOr;
using MediatR;
using DeveloperStore.Shared.Contracts.Dtos;

namespace DeveloperStore.Users.Application.Queries.GetUserById;

public class GetUserByIdQuery : IRequest<ErrorOr<UserDto>>
{
    public int Id { get; }

    public GetUserByIdQuery(int id)
    {
        Id = id;
    }
}