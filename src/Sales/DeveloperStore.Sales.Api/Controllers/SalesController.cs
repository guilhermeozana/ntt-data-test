using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DeveloperStore.Sales.Application.Commands.CancelSale;
using DeveloperStore.Sales.Application.Commands.CreateSale;
using DeveloperStore.Sales.Application.Commands.UpdateSale;
using DeveloperStore.Sales.Application.Queries.GetAllSales;
using DeveloperStore.Sales.Application.Queries.GetSaleById;
using DeveloperStore.Sales.Contracts.Requests;
using DeveloperStore.Shared.Api.Queries;
using DeveloperStore.Shared.SharedKernel.Dtos;
using DeveloperStore.Shared.Api.Extensions;

namespace DeveloperStore.Sales.Api.Controllers;

[ApiController]
[Route("sales")]
[Authorize]
public class SalesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public SalesController(IMediator mediator, IMapper mapper)
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

        var result = await _mediator.Send(new GetAllSalesQuery(criteria));

        return result.Match(
            sales => Ok(sales),
            this.Problem
        );
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetSaleByIdQuery(id);
        var result = await _mediator.Send(query);
        return result.Match(
            sale => Ok(sale),
            this.Problem
        );
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateSaleRequest request)
    {
        var command = _mapper.Map<CreateSaleCommand>(request);
        var result = await _mediator.Send(command);

        return result.Match(
            sale => CreatedAtAction(
                nameof(GetById),
                new { id = sale.Id },
                sale),
            this.Problem);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSaleRequest request)
    {
        var command = _mapper.Map<UpdateSaleCommand>(request);
        command.Id = id;

        var result = await _mediator.Send(command);

        return result.Match(
            sale => Ok(sale),
            this.Problem
        );
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Cancel(int id)
    {
        var command = new CancelSaleCommand(id);
        var result = await _mediator.Send(command);
        return result.Match(
            cancelled => Ok(cancelled),
            this.Problem
        );
    }
}