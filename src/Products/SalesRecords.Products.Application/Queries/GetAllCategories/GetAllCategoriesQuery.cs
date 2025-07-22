using ErrorOr;
using MediatR;
using SalesRecords.Products.Domain.Entities;

namespace SalesRecords.Products.Application.Queries.GetAllCategories;

public class GetAllCategoriesQuery: IRequest<ErrorOr<List<string>>>;