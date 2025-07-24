namespace DeveloperStore.Sales.Contracts.Requests;

public class CreateSaleRequest
{
    public int CustomerId { get; set; }
    public int BranchId { get; set; }
    public string Date { get; set; }
    public List<SaleItemRequest> Products { get; set; }
}