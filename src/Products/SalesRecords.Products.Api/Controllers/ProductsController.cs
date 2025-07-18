using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SalesRecords.Products.Api.Common.Errors;
using SalesRecords.Products.Api.Contracts;
using SalesRecords.Products.Application.Commands;
using SalesRecords.Products.Application.Commands.CreateProduct;
using SalesRecords.Products.Application.Commands.UpdateProduct;
using SalesRecords.Products.Application.Queries.GetAllCategories;
using SalesRecords.Products.Application.Queries.GetAllProducts;
using SalesRecords.Products.Application.Queries.GetProductById;
using SalesRecords.Products.Application.Queries.GetProductsByCategory;

namespace SalesRecords.Products.Api.Controllers;

[ApiController]
[Route("products")]
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
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllProductsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetProductByIdQuery(id);
        var result = await _mediator.Send(query);
        return Ok(result);
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
            errors => this.Problem(errors));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductRequest request)
    {
        var command = _mapper.Map<UpdateProductCommand>(request);
        command.Id = id;
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteProductCommand(id);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories()
    {
        var query = new GetAllCategoriesQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("category/{category}")]
    public async Task<IActionResult> GetByCategory(string category, [FromQuery] GetProductsByCategoryRequest request)
    {
        request.Category = category;
        var query = _mapper.Map<GetProductsByCategoryQuery>(request);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
