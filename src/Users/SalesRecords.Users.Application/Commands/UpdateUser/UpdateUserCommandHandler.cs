using AutoMapper;
using ErrorOr;
using MediatR;
using SalesRecords.Users.Contracts.Dtos;
using SalesRecords.Users.Domain.Entities;
using SalesRecords.Users.Application.Common.Interfaces;
using SalesRecords.Users.Domain.Enums;
using SalesRecords.Users.Domain.ValueObjects;

namespace SalesRecords.Users.Application.Commands.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, ErrorOr<UserDto>>
{
    private readonly IUserRepository _repo;
    private readonly IMapper _mapper;

    public UpdateUserCommandHandler(IUserRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<ErrorOr<UserDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _repo.GetByIdAsync(request.Id);

        if (existingUser is null)
        {
            return Error.NotFound("User.NotFound", "Usuário não encontrado.");
        }

        existingUser.Update(
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

        await _repo.UpdateAsync(existingUser);
        var userDto = _mapper.Map<UserDto>(existingUser);

        return userDto;
    }
}