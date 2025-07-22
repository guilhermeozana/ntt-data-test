using SalesRecords.Carts.Contracts.Dtos;

namespace SalesRecords.Carts.Contracts.Requests;

public class CreateCartRequest
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Date { get; set; }
    public List<CartItemDto> Products { get; set; }
}
