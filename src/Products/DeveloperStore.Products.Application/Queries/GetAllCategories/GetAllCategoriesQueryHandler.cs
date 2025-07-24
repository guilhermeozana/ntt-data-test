using AutoMapper;
using ErrorOr;
using MediatR;
using DeveloperStore.Products.Application.Common.Interfaces;
using DeveloperStore.Products.Application.Queries.GetAllProducts;
using DeveloperStore.Products.Contracts.Dtos;
using DeveloperStore.Products.Domain.Entities;

namespace DeveloperStore.Products.Application.Queries.GetAllCategories;

public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, ErrorOr<List<string>>>
{
    private readonly IProductRepository _repo;
    private readonly IMapper _mapper;

    public GetAllCategoriesQueryHandler(IProductRepository repo)
    {
        _repo = repo;
    }

    public async Task<ErrorOr<List<string>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _repo.GetAllCategoriesAsync();
        
        return categories;
    }
}