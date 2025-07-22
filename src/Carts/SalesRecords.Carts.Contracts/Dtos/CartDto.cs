namespace SalesRecords.Carts.Contracts.Dtos;

public class CartDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Date { get; set; }
    public List<CartItemDto> Products { get; set; } = new List<CartItemDto>();
}