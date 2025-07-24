using AutoMapper;
using MediatR;
using ErrorOr;
using DeveloperStore.Shared.Contracts.Dtos;
using DeveloperStore.Shared.SharedKernel.Responses;
using DeveloperStore.Users.Application.Common.Repositories;

namespace DeveloperStore.Users.Application.Queries.GetAllUsers;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, ErrorOr<PagedResponse<UserDto>>>
{
    private readonly IUserRepository _repo;
    private readonly IMapper _mapper;

    public GetAllUsersQueryHandler(IUserRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<ErrorOr<PagedResponse<UserDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var result = await _repo.GetAllAsync(request.Criteria);
        var userDtos = _mapper.Map<List<UserDto>>(result.Items);
        var totalPages = (int)Math.Ceiling((double)result.TotalCount / request.Criteria.Size);

        var pagedResponse = new PagedResponse<UserDto>
        {
            Data = userDtos,
            TotalItems = result.TotalCount,
            CurrentPage = request.Criteria.Page,
            TotalPages = totalPages
        };

        return pagedResponse;
    }
}