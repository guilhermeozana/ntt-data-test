using AutoMapper;
using ErrorOr;
using FluentAssertions;
using NSubstitute;
using DeveloperStore.Sales.Application.Common.Repositories;
using DeveloperStore.Sales.Application.Queries.GetSaleById;
using DeveloperStore.Sales.Contracts.Dtos;
using DeveloperStore.Sales.Domain.Entities;

namespace DeveloperStore.Sales.UnitTests.Application.Queries;

public class GetSaleByIdQueryHandlerTests
{
    private readonly ISaleRepository _repo = Substitute.For<ISaleRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private GetSaleByIdQueryHandler CreateHandler() => new(_repo, _mapper);

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenSaleDoesNotExist()
    {
        var query = new GetSaleByIdQuery(99);

        _repo.GetByIdAsync(query.Id).Returns((Sale?)null);

        var handler = CreateHandler();
        var result = await handler.Handle(query, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        result.FirstError.Code.Should().Be("Sale.NotFound");
    }

    [Fact]
    public async Task Handle_ShouldReturnSaleDto_WhenSaleExists()
    {
        var sale = Sale.Create("S-001", DateTime.Today, customerId: 1, branchId: 1);
        var query = new GetSaleByIdQuery(sale.Id);

        _repo.GetByIdAsync(query.Id).Returns(sale);

        _mapper.Map<SaleDto>(sale).Returns(new SaleDto { SaleNumber = "S-001" });

        var handler = CreateHandler();
        var result = await handler.Handle(query, default);

        result.IsError.Should().BeFalse();
        result.Value.SaleNumber.Should().Be("S-001");
    }
}