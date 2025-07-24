using DeveloperStore.Carts.Contracts.Dtos;

namespace DeveloperStore.Carts.Contracts.Requests
{
    public class UpdateCartRequest
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Date { get; set; }
        public List<CartItemDto> Products { get; set; }
    }
}