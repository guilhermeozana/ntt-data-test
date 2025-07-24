using AutoMapper;
using DeveloperStore.Sales.Application.Commands.CreateSale;
using DeveloperStore.Sales.Application.Commands.UpdateSale;
using DeveloperStore.Sales.Contracts.Dtos;
using DeveloperStore.Sales.Contracts.Requests;
using DeveloperStore.Sales.Domain.Entities;
using DeveloperStore.Sales.Domain.ValueObjects;

namespace DeveloperStore.Sales.Api.Mapping;

public class SaleMappingProfile : Profile
{
    public SaleMappingProfile()
    {
        CreateMap<CreateSaleRequest, CreateSaleCommand>();
        CreateMap<UpdateSaleRequest, UpdateSaleCommand>();
        CreateMap<SaleItemRequest, SaleItemDto>();
        CreateMap<Sale, SaleDto>();
        CreateMap<SaleItem, SaleItemDto>();
    }
}
