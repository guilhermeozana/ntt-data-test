using MediatR;
using SalesRecords.Products.Contracts.Dtos;

namespace SalesRecords.Products.Application.Queries.GetProductsByCategory;

public class GetProductsByCategoryQuery : IRequest<List<ProductDto>>
{
    
}