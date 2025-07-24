using ErrorOr;
using MediatR;
using DeveloperStore.Sales.Contracts.Dtos;

namespace DeveloperStore.Sales.Application.Commands.UpdateSale;

public class UpdateSaleCommand : IRequest<ErrorOr<SaleDto>>
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int BranchId { get; set; }
    public string Date { get; set; }
    public List<SaleItemDto> Products { get; set; } = new();
}