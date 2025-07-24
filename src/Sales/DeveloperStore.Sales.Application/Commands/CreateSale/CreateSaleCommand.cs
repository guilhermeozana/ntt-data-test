using ErrorOr;
using MediatR;
using DeveloperStore.Sales.Contracts.Dtos;

namespace DeveloperStore.Sales.Application.Commands.CreateSale;

public class CreateSaleCommand : IRequest<ErrorOr<SaleDto>>
{
    public int CustomerId { get; set; }
    public int BranchId { get; set; }
    public string Date { get; set; }
    public List<SaleItemDto> Products { get; set; } = new();
}