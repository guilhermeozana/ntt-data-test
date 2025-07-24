using AutoMapper;
using DeveloperStore.Shared.Contracts.Dtos;
using DeveloperStore.Users.Application.Commands.CreateUser;
using DeveloperStore.Users.Application.Commands.UpdateUser;
using DeveloperStore.Users.Contracts.Requests;
using DeveloperStore.Users.Domain.Entities;
using DeveloperStore.Users.Domain.ValueObjects;

namespace DeveloperStore.Users.Api.Mapping;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));

        CreateMap<Name, NameDto>();
        CreateMap<Geolocation, GeolocationDto>();
        CreateMap<Address, AddressDto>();
        CreateMap<CreateUserRequest, CreateUserCommand>();
        CreateMap<UpdateUserRequest, UpdateUserCommand>();

    }
}