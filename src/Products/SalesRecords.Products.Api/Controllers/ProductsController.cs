using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesRecords.Products.Application.Commands.CreateProduct;
using SalesRecords.Products.Application.Commands.DeleteProduct;
using SalesRecords.Products.Application.Commands.UpdateProduct;
using SalesRecords.Products.Application.Queries.GetAllCategories;
using SalesRecords.Products.Application.Queries.GetAllProducts;
using SalesRecords.Products.Application.Queries.GetProductById;
using SalesRecords.Products.Application.Queries.GetProductsByCategory;
using SalesRecords.Products.Contracts.Dtos;
using SalesRecords.Products.Contracts.Requests;
using SalesRecords.Shared.SharedKernel.Dtos;
using SalesRecords.Shared.Api.Extensions;
using SalesRecords.Shared.Api.Queries;

namespace SalesRecords.Products.Api.Controllers;

[ApiController]
[Route("products")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ProductsController(IMediator mediator, IMapper mapper)
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

        var result = await _mediator.Send(new GetAllProductsQuery(criteria));

        return result.Match(
            products => Ok(products),
            this.Problem
        );
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetProductByIdQuery(id);
        var result = await _mediator.Send(query);
        return result.Match(
            products => Ok(products),
            this.Problem
        );
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductRequest request)
    {
        var command = _mapper.Map<CreateProductCommand>(request);
        var result = await _mediator.Send(command);

        return result.Match(
            product => CreatedAtAction(
                nameof(GetById),
                new { id = product.Id },
                product),
            this.Problem);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductRequest request)
    {
        var command = _mapper.Map<UpdateProductCommand>(request);
        command.Id = id;

        var result = await _mediator.Send(command);

        return result.Match(
            product => Ok(product),
            this.Problem
        );
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteProductCommand(id);
        var result = await _mediator.Send(command);
        return result.Match(
            deleted => Ok(deleted),
            this.Problem
        );
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetAllCategories()
    {
        var query = new GetAllCategoriesQuery();
        var result = await _mediator.Send(query);

        return result.Match(
            categories => Ok(categories),
            this.Problem
        );
    }

    [HttpGet("category/{category}")]
    public async Task<IActionResult> GetByCategory(
        [FromRoute] string category,
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

        var query = new GetProductsByCategoryQuery(
            category: category, criteria: criteria);

        var result = await _mediator.Send(query);

        return result.Match(
            products => Ok(products),
            this.Problem
        );
    }
}
