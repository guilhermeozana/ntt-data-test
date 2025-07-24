using AutoMapper;
using DeveloperStore.Products.Application.Commands.CreateProduct;
using DeveloperStore.Products.Application.Commands.UpdateProduct;
using DeveloperStore.Products.Contracts.Dtos;
using DeveloperStore.Products.Contracts.Requests;
using DeveloperStore.Products.Domain.Entities;
using DeveloperStore.Products.Domain.ValueObjects;

namespace DeveloperStore.Products.Api.Mapping;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<CreateProductRequest, CreateProductCommand>();
        CreateMap<UpdateProductRequest, UpdateProductCommand>();
        CreateMap<Product, ProductDto>();
        CreateMap<Rating, RatingDto>();

    }
}
