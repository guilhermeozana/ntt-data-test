using AutoMapper;
using ErrorOr;
using MediatR;
using DeveloperStore.Sales.Application.Common.Events;
using DeveloperStore.Sales.Application.Common.Repositories;
using DeveloperStore.Sales.Contracts.Dtos;

namespace DeveloperStore.Sales.Application.Commands.UpdateSale;

public class UpdateSaleCommandHandler : IRequestHandler<UpdateSaleCommand, ErrorOr<SaleDto>>
{
    private readonly ISaleRepository _repo;
    private readonly IMapper _mapper;
    private readonly IDomainEventDispatcher _dispatcher;

    public UpdateSaleCommandHandler(ISaleRepository repo, IMapper mapper, IDomainEventDispatcher dispatcher)
    {
        _repo = repo;
        _mapper = mapper;
        _dispatcher = dispatcher;
    }

    public async Task<ErrorOr<SaleDto>> Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = await _repo.GetByIdAsync(request.Id);
        if (sale is null)
            return Error.NotFound("Sale.NotFound", $"Sale with id {request.Id} was not found.");

        sale.Update(DateTime.Parse(request.Date).ToUniversalTime(), request.CustomerId, request.BranchId);

        var products = request.Products;

        sale.ClearItems();

        foreach (var product in products)
        {
            var result = sale.AddItem(
                product.ProductId,
                product.Quantity,
                product.UnitPrice
            );

            if (result.IsError)
                return result.Errors;
        }

        await _repo.UpdateAsync(sale);
        await _dispatcher.DispatchEventsAsync(sale.DomainEvents);
        sale.ClearDomainEvents();

        var dto = _mapper.Map<SaleDto>(sale);
        return dto;
    }
}