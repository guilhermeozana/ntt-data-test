using AutoMapper;
using SalesRecords.Auth.Application.Commands.Login;
using SalesRecords.Auth.Contracts.Requests;

namespace SalesRecords.Auth.Api.Mapping;

public class AuthMappingProfile : Profile
{
    public AuthMappingProfile()
    {
        CreateMap<LoginRequest, LoginCommand>();
    }
}
