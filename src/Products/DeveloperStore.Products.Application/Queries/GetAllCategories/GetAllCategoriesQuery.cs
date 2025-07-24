using ErrorOr;
using MediatR;
using DeveloperStore.Products.Domain.Entities;

namespace DeveloperStore.Products.Application.Queries.GetAllCategories;

public class GetAllCategoriesQuery: IRequest<ErrorOr<List<string>>>;