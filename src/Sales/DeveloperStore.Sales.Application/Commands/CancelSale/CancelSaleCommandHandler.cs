using ErrorOr;
using MediatR;
using DeveloperStore.Sales.Application.Common.Events;
using DeveloperStore.Sales.Application.Common.Repositories;

namespace DeveloperStore.Sales.Application.Commands.CancelSale;

public class CancelSaleCommandHandler : IRequestHandler<CancelSaleCommand, ErrorOr<Success>>
{
    private readonly ISaleRepository _repo;
    private readonly IDomainEventDispatcher _dispatcher;

    public CancelSaleCommandHandler(ISaleRepository repo, IDomainEventDispatcher dispatcher)
    {
        _repo = repo;
        _dispatcher = dispatcher;
    }

    public async Task<ErrorOr<Success>> Handle(CancelSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = await _repo.GetByIdAsync(request.Id);

        if (sale is null)
        {
            return Error.NotFound("Sale.NotFound", $"Sale with id {request.Id} was not found.");
        }

        sale.Cancel();

        await _repo.UpdateAsync(sale);
        await _dispatcher.DispatchEventsAsync(sale.DomainEvents);
        sale.ClearDomainEvents();

        return Result.Success;
    }
}