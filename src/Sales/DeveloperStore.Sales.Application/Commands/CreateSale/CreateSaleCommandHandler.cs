using AutoMapper;
using ErrorOr;
using MediatR;
using DeveloperStore.Sales.Application.Commands.CreateSale;
using DeveloperStore.Sales.Application.Common.Events;
using DeveloperStore.Sales.Application.Common.Repositories;
using DeveloperStore.Sales.Contracts.Dtos;
using DeveloperStore.Sales.Domain.Entities;
using DeveloperStore.Shared.SharedKernel.Domain.Events;

namespace DeveloperStore.Sales.Application.Commands.CreateSale;

public class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, ErrorOr<SaleDto>>
{
    private readonly ISaleRepository _repo;
    private readonly IMapper _mapper;
    private readonly IDomainEventDispatcher _dispatcher;

    public CreateSaleCommandHandler(
        ISaleRepository repo,
        IMapper mapper,
        IDomainEventDispatcher dispatcher)
    {
        _repo = repo;
        _mapper = mapper;
        _dispatcher = dispatcher;
    }

    public async Task<ErrorOr<SaleDto>> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = Sale.Create(
            saleNumber: GenerateSaleNumber(),
            date: DateTime.Parse(request.Date).ToUniversalTime(),
            customerId: request.CustomerId,
            branchId: request.BranchId
        );

        foreach (var item in request.Products)
        {
            var result = sale.AddItem(
                productId: item.ProductId,
                quantity: item.Quantity,
                unitPrice: item.UnitPrice
            );

            if (result.IsError)
                return result.Errors;
        }

        await _repo.AddAsync(sale);

        await _dispatcher.DispatchEventsAsync(sale.DomainEvents);
        sale.ClearDomainEvents();

        var saleDto = _mapper.Map<SaleDto>(sale);
        return saleDto;
    }

    private string GenerateSaleNumber()
    {
        return $"S-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}";
    }
}