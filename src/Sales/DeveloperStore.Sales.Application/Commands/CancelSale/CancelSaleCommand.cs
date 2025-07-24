using ErrorOr;
using MediatR;

namespace DeveloperStore.Sales.Application.Commands.CancelSale;

public sealed class CancelSaleCommand : IRequest<ErrorOr<Success>>
{
    public int Id { get; }

    public CancelSaleCommand(int id)
    {
        Id = id;
    }
}