using MediatR;
using ErrorOr;
using SalesRecords.Shared.SharedKernel.Dtos;
using SalesRecords.Shared.SharedKernel.Responses;
using SalesRecords.Users.Contracts.Dtos;

namespace SalesRecords.Users.Application.Queries.GetAllUsers;

public class GetAllUsersQuery : IRequest<ErrorOr<PagedResponse<UserDto>>>
{
    public QueryCriteriaDto Criteria { get; }

    public GetAllUsersQuery(QueryCriteriaDto criteria)
    {
        Criteria = criteria;
    }
}