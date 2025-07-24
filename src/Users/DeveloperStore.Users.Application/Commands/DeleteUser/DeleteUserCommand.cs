using ErrorOr;
using MediatR;

namespace DeveloperStore.Users.Application.Commands.DeleteUser;

public class DeleteUserCommand : IRequest<ErrorOr<Deleted>>
{
    public int Id { get; }

    public DeleteUserCommand(int id)
    {
        Id = id;
    }
}