using ErrorOr;
using MediatR;
using SalesRecords.Users.Application.Common.Interfaces;

namespace SalesRecords.Users.Application.Commands.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, ErrorOr<Deleted>>
{
    private readonly IUserRepository _repo;

    public DeleteUserCommandHandler(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task<ErrorOr<Deleted>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var product = await _repo.GetByIdAsync(request.Id);

        if (product == null)
        {
            throw new ArgumentException($"Product with id {request.Id} does not exist");
        }
        
        var deleted = await _repo.DeleteAsync(product);
        
        return deleted ? Result.Deleted : Error.Unexpected("Failed to delete product");
    }
}