using AutoMapper;
using DeveloperStore.Carts.Application.Commands.CreateCart;
using DeveloperStore.Carts.Application.Commands.UpdateCart;
using DeveloperStore.Carts.Contracts.Dtos;
using DeveloperStore.Carts.Contracts.Requests;
using DeveloperStore.Carts.Domain.Entities;
using DeveloperStore.Carts.Domain.ValueObjects;

namespace DeveloperStore.Carts.Api.Mapping;

public class CartMappingProfile : Profile
{
    public CartMappingProfile()
    {
        CreateMap<CreateCartRequest, CreateCartCommand>();
        CreateMap<UpdateCartRequest, UpdateCartCommand>();
        CreateMap<Cart, CartDto>();
        CreateMap<CartItem, CartItemDto>();
    }
}
