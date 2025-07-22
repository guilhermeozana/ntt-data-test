using AutoMapper;
using ErrorOr;
using MediatR;
using SalesRecords.Products.Application.Common.Interfaces;
using SalesRecords.Products.Application.Queries.GetAllProducts;
using SalesRecords.Products.Contracts.Dtos;
using SalesRecords.Products.Domain.Entities;

namespace SalesRecords.Products.Application.Queries.GetAllCategories;

public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, ErrorOr<List<string>>>
{
    private readonly IProductRepository _repo;
    private readonly IMapper _mapper;

    public GetAllCategoriesQueryHandler(IProductRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<ErrorOr<List<string>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _repo.GetAllCategoriesAsync();
        
        return categories;
    }
}