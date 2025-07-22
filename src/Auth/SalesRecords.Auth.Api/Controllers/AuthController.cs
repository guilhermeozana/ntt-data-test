using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SalesRecords.Auth.Application.Commands.Login;
using SalesRecords.Auth.Contracts.Requests;
using SalesRecords.Auth.Contracts.Response;
using SalesRecords.Shared.Api.Extensions;

namespace SalesRecords.Auth.Api.Controllers;

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
