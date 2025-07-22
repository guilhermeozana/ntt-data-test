using AutoMapper;
using SalesRecords.Users.Application.Commands.CreateUser;
using SalesRecords.Users.Application.Commands.UpdateUser;
using SalesRecords.Users.Contracts.Requests;

namespace SalesRecords.Users.Api.Mapping;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<CreateUserRequest, CreateUserCommand>();
        CreateMap<UpdateUserRequest, UpdateUserCommand>();
    }
}
