using MediatR;
using ErrorOr;
using DeveloperStore.Shared.Contracts.Dtos;
using DeveloperStore.Shared.SharedKernel.Dtos;
using DeveloperStore.Shared.SharedKernel.Responses;

namespace DeveloperStore.Users.Application.Queries.GetAllUsers;

public class GetAllUsersQuery : IRequest<ErrorOr<PagedResponse<UserDto>>>
{
    public QueryCriteriaDto Criteria { get; }

    public GetAllUsersQuery(QueryCriteriaDto criteria)
    {
        Criteria = criteria;
    }
}