using ErrorOr;
using MediatR;

namespace SalesRecords.Users.Application.Commands.DeleteUser;

public class DeleteUserCommand : IRequest<ErrorOr<Deleted>>
{
    public int Id { get; }

    public DeleteUserCommand(int id)
    {
        Id = id;
    }
}