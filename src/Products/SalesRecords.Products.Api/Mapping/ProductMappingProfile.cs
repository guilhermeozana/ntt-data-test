using SalesRecords.Products.Application.Commands.CreateProduct;
using SalesRecords.Products.Application.Commands.UpdateProduct;
using SalesRecords.Products.Application.Queries.GetProductsByCategory;

namespace SalesRecords.Products.Api.Mapping;

using AutoMapper;
using SalesRecords.Products.Api.Contracts;
using SalesRecords.Products.Application.Commands;
using SalesRecords.Products.Application.Queries;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<CreateProductRequest, CreateProductCommand>();
        CreateMap<UpdateProductRequest, UpdateProductCommand>();
        CreateMap<GetProductsByCategoryRequest, GetProductsByCategoryQuery>();
    }
}
