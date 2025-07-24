using AutoMapper;
using ErrorOr;
using MediatR;
using DeveloperStore.Sales.Application.Common.Repositories;
using DeveloperStore.Sales.Contracts.Dtos;

namespace DeveloperStore.Sales.Application.Queries.GetSaleById;

public class GetSaleByIdQueryHandler : IRequestHandler<GetSaleByIdQuery, ErrorOr<SaleDto>>
{
    private readonly ISaleRepository _repo;
    private readonly IMapper _mapper;

    public GetSaleByIdQueryHandler(ISaleRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<ErrorOr<SaleDto>> Handle(GetSaleByIdQuery request, CancellationToken cancellationToken)
    {
        var sale = await _repo.GetByIdAsync(request.Id);
        if (sale is null)
            return Error.NotFound("Sale.NotFound", $"Sale with ID {request.Id} was not found.");

        var saleDto = _mapper.Map<SaleDto>(sale);

        return saleDto;
    }
}