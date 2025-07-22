using AutoMapper;
using ErrorOr;
using MediatR;
using SalesRecords.Users.Contracts.Dtos;
using SalesRecords.Users.Domain.Entities;
using SalesRecords.Users.Application.Common.Interfaces;

namespace SalesRecords.Users.Application.Queries.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, ErrorOr<UserDto>>
{
    private readonly IUserRepository _repo;
    private readonly IMapper _mapper;

    public GetUserByIdQueryHandler(IUserRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<ErrorOr<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _repo.GetByIdAsync(request.Id);
        if (user is null)
            return Error.NotFound(code: "User.NotFound", description: "Usuário não encontrado.");

        var userDto = _mapper.Map<UserDto>(user);
        return userDto;
    }
}