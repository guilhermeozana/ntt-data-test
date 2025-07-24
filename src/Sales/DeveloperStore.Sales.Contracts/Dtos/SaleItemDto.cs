namespace DeveloperStore.Sales.Contracts.Dtos;

public class SaleItemDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}