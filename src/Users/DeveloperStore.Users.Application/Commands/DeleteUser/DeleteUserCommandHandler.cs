using DeveloperStore.Users.Application.Common.Repositories;
using ErrorOr;
using MediatR;

namespace DeveloperStore.Users.Application.Commands.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, ErrorOr<Deleted>>
{
    private readonly IUserRepository _repo;

    public DeleteUserCommandHandler(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task<ErrorOr<Deleted>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _repo.GetByIdAsync(request.Id);

        if (user == null)
            return Error.Validation("User.Delete.NotFound", $"User with id {request.Id} does not exist");        
        
        var deleted = await _repo.DeleteAsync(user);
        
        return deleted ? Result.Deleted : Error.Unexpected("User.Delete.FailedToDelete", "Failed to delete user");
    }
}