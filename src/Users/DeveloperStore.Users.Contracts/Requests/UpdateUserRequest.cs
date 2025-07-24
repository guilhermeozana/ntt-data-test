using DeveloperStore.Shared.Contracts.Dtos;

namespace DeveloperStore.Users.Contracts.Requests
{
    public class UpdateUserRequest
    {
        public int Id { get; set; }
        public string Email { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
        public NameDto Name { get; set; } = new();
        public AddressDto Address { get; set; } = new();
        public string Phone { get; set; } = default!;
        public string Status { get; set; } = "Active";
        public string Role { get; set; } = "Customer";
    }
}