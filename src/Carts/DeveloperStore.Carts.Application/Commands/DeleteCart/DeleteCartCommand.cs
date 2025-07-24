using ErrorOr;
using MediatR;

namespace DeveloperStore.Carts.Application.Commands.DeleteCart;

public sealed class DeleteCartCommand : IRequest<ErrorOr<Deleted>>
{
    public int Id { get; }

    public DeleteCartCommand(int id)
    {
        Id = id;
    }
}