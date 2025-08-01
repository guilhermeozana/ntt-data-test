using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DeveloperStore.Shared.Api.Queries;
using DeveloperStore.Shared.Api.Extensions;
using DeveloperStore.Shared.SharedKernel.Dtos;
using DeveloperStore.Users.Application.Commands.CreateUser;
using DeveloperStore.Users.Application.Commands.DeleteUser;
using DeveloperStore.Users.Application.Commands.UpdateUser;
using DeveloperStore.Users.Application.Queries.GetAllUsers;
using DeveloperStore.Users.Application.Queries.GetUserById;
using DeveloperStore.Users.Application.Queries.GetUserByUsername;
using DeveloperStore.Users.Contracts.Requests;

namespace DeveloperStore.Users.Api.Controllers;

[ApiController]
[Route("users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public UsersController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [ModelBinder(BinderType = typeof(RequestQueryOptionsBinder))] RequestQueryOptions request)
    {
        var criteria = new QueryCriteriaDto(
            page: request.Page,
            size: request.Size,
            order: request.Order,
            filters: request.Filters,
            minValues: request.MinValues,
            maxValues: request.MaxValues
        );

        var result = await _mediator.Send(new GetAllUsersQuery(criteria));

        return result.Match(
            users => Ok(users),
            this.Problem
        );
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetUserByIdQuery(id);
        var result = await _mediator.Send(query);
        return result.Match(
            users => Ok(users),
            this.Problem
        );
    }
    
    [HttpGet("byUsername/{username}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByUsername(string username)
    {
        var query = new GetUserByUsernameQuery(username);
        var result = await _mediator.Send(query);
        return result.Match(
            users => Ok(users),
            this.Problem
        );
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserRequest request)
    {
        var command = _mapper.Map<CreateUserCommand>(request);
        var result = await _mediator.Send(command);

        return result.Match(
            user => CreatedAtAction(
                nameof(GetById),
                new { id = user.Id },
                user),
            this.Problem);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserRequest request)
    {
        var command = _mapper.Map<UpdateUserCommand>(request);
        command.Id = id;

        var result = await _mediator.Send(command);

        return result.Match(
            user => Ok(user),
            this.Problem
        );
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteUserCommand(id);
        var result = await _mediator.Send(command);
        return result.Match(
            deleted => Ok(deleted),
            this.Problem
        );
    }
}
