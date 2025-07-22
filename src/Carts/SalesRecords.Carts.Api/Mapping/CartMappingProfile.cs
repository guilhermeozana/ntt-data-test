using AutoMapper;
using SalesRecords.Carts.Application.Commands.CreateCart;
using SalesRecords.Carts.Application.Commands.UpdateCart;
using SalesRecords.Carts.Contracts.Requests;

namespace SalesRecords.Carts.Api.Mapping;

public class CartMappingProfile : Profile
{
    public CartMappingProfile()
    {
        CreateMap<CreateCartRequest, CreateCartCommand>();
        CreateMap<UpdateCartRequest, UpdateCartCommand>();
    }
}
