using AutoMapper;
using SalesRecords.Products.Application.Commands.CreateProduct;
using SalesRecords.Products.Application.Commands.UpdateProduct;
using SalesRecords.Products.Contracts.Requests;

namespace SalesRecords.Products.Api.Mapping;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<CreateProductRequest, CreateProductCommand>();
        CreateMap<UpdateProductRequest, UpdateProductCommand>();
    }
}
