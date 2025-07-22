using AutoMapper;
using ErrorOr;
using MediatR;
using SalesRecords.Users.Application.Common.Interfaces;
using SalesRecords.Users.Contracts.Dtos;
using SalesRecords.Users.Domain.Entities;
using SalesRecords.Users.Domain.Enums;
using SalesRecords.Users.Domain.ValueObjects;

namespace SalesRecords.Users.Application.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ErrorOr<UserDto>>
{
    private readonly IUserRepository _repo;
    private readonly IMapper _mapper;

    public CreateUserCommandHandler(IUserRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<ErrorOr<UserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = User.Create(
            email: request.Email,
            username: request.Username,
            password: request.Password,
            name: new Name(request.Name.Firstname, request.Name.Lastname),
            address: new Address(
                request.Address.City,
                request.Address.Street,
                request.Address.Number,
                request.Address.Zipcode,
                new Geolocation(request.Address.Geolocation.Lat, request.Address.Geolocation.Long)
            ),
            phone: request.Phone,
            status: Enum.Parse<UserStatus>(request.Status, ignoreCase: true),
            role: Enum.Parse<UserRole>(request.Role, ignoreCase: true)
        );

        await _repo.AddAsync(user);

        var userDto = _mapper.Map<UserDto>(user);

        return userDto;
    }
}