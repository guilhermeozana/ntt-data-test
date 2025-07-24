using AutoMapper;
using DeveloperStore.Shared.Contracts.Dtos;
using DeveloperStore.Users.Application.Common.Repositories;
using ErrorOr;
using MediatR;

namespace DeveloperStore.Users.Application.Queries.GetUserByUsername;

public class GetUserByUsernameQueryHandler : IRequestHandler<GetUserByUsernameQuery, ErrorOr<UserDto>>
{
    private readonly IUserRepository _repo;
    private readonly IMapper _mapper;

    public GetUserByUsernameQueryHandler(IUserRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<ErrorOr<UserDto>> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken)
    {
        var user = await _repo.GetByUsernameAsync(request.Username);
        if (user is null)
            return Error.NotFound(code: "User.NotFound", description: "User not found.");

        var userDto = _mapper.Map<UserDto>(user);

        return userDto;
    }
}