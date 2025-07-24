using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using DeveloperStore.Auth.Application.Commands.Login;
using DeveloperStore.Auth.Contracts.Requests;
using DeveloperStore.Auth.Contracts.Response;
using DeveloperStore.Shared.Api.Extensions;

namespace DeveloperStore.Auth.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public AuthController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var command = _mapper.Map<LoginCommand>(request);
        var result = await _mediator.Send(command);

        return result.Match(
            token => Ok(new LoginResponse(token)),
            this.Problem);
    }
}
