using ErrorOr;
using MediatR;
using SalesRecords.Products.Contracts.Dtos;

namespace SalesRecords.Products.Application.Queries.GetAllProducts;

public class GetAllProductsQuery : IRequest<ErrorOr<List<ProductDto>>>;