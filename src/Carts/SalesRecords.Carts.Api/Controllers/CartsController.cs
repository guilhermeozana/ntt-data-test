using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesRecords.Shared.SharedKernel.Dtos;
using SalesRecords.Carts.Application.Commands.CreateCart;
using SalesRecords.Carts.Application.Commands.DeleteCart;
using SalesRecords.Carts.Application.Commands.UpdateCart;
using SalesRecords.Carts.Application.Queries.GetAllCarts;
using SalesRecords.Carts.Application.Queries.GetCartById;
using SalesRecords.Carts.Contracts.Requests;
using SalesRecords.Shared.Api.Queries;
using SalesRecords.Shared.Api.Extensions;

namespace SalesRecords.Carts.Api.Controllers;

[ApiController]
[Route("carts")]
[Authorize]
public class CartsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CartsController(IMediator mediator, IMapper mapper)
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

        var result = await _mediator.Send(new GetAllCartsQuery(criteria));

        return result.Match(
            carts => Ok(carts),
            this.Problem
        );
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetCartByIdQuery(id);
        var result = await _mediator.Send(query);
        return result.Match(
            carts => Ok(carts),
            this.Problem
        );
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCartRequest request)
    {
        var command = _mapper.Map<CreateCartCommand>(request);
        var result = await _mediator.Send(command);

        return result.Match(
            cart => CreatedAtAction(
                nameof(GetById),
                new { id = cart.Id },
                cart),
            this.Problem);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCartRequest request)
    {
        var command = _mapper.Map<UpdateCartCommand>(request);
        command.Id = id;

        var result = await _mediator.Send(command);

        return result.Match(
            cart => Ok(cart),
            this.Problem
        );
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteCartCommand(id);
        var result = await _mediator.Send(command);
        return result.Match(
            deleted => Ok(deleted),
            this.Problem
        );
    }
}
