namespace DeveloperStore.Sales.Contracts.Requests;

public class SaleItemRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}