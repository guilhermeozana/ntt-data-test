using ErrorOr;
using MediatR;
using SalesRecords.Users.Contracts.Dtos;

namespace SalesRecords.Users.Application.Queries.GetUserById;

public class GetUserByIdQuery : IRequest<ErrorOr<UserDto>>
{
    public int Id { get; }

    public GetUserByIdQuery(int id)
    {
        Id = id;
    }
}