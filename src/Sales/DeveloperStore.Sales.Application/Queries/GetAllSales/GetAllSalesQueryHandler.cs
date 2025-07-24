using AutoMapper;
using ErrorOr;
using MediatR;
using DeveloperStore.Sales.Application.Common.Repositories;
using DeveloperStore.Sales.Contracts.Dtos;
using DeveloperStore.Shared.SharedKernel.Responses;

namespace DeveloperStore.Sales.Application.Queries.GetAllSales;

public class GetAllSalesQueryHandler : IRequestHandler<GetAllSalesQuery, ErrorOr<PagedResponse<SaleDto>>>
{
    private readonly ISaleRepository _repo;
    private readonly IMapper _mapper;

    public GetAllSalesQueryHandler(ISaleRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<ErrorOr<PagedResponse<SaleDto>>> Handle(GetAllSalesQuery request, CancellationToken cancellationToken)
    {
        var result = await _repo.GetPagedAsync(request.Criteria);

        var saleDtos = _mapper.Map<List<SaleDto>>(result.Items);

        var totalPages = (int)Math.Ceiling((double)result.TotalCount / request.Criteria.Size);

        var pagedResponse = new PagedResponse<SaleDto>
        {
            Data = saleDtos,
            TotalItems = result.TotalCount,
            CurrentPage = request.Criteria.Page,
            TotalPages = totalPages
        };

        return pagedResponse;
    }
}