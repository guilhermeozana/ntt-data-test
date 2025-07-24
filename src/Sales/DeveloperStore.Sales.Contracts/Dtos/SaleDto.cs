namespace DeveloperStore.Sales.Contracts.Dtos;

public class SaleDto
{
    public int Id { get; set; }
    public string SaleNumber { get; set; }
    public int CustomerId { get; set; }
    public int BranchId { get; set; }
    public string Date { get; set; }
    public bool IsCancelled { get; set; }
    public decimal TotalAmount { get; set; }
    public List<SaleItemDto> Products { get; set; }
}