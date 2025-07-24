using AutoMapper;
using DeveloperStore.Auth.Application.Commands.Login;
using DeveloperStore.Auth.Contracts.Requests;

namespace DeveloperStore.Auth.Api.Mapping;

public class AuthMappingProfile : Profile
{
    public AuthMappingProfile()
    {
        CreateMap<LoginRequest, LoginCommand>();
    }
}
