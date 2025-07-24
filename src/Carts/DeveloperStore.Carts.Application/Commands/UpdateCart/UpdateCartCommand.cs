using ErrorOr;
using MediatR;
using DeveloperStore.Carts.Contracts.Dtos;

namespace DeveloperStore.Carts.Application.Commands.UpdateCart;

public class UpdateCartCommand : IRequest<ErrorOr<CartDto>>
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Date { get; set; }
    public List<CartItemDto> Products { get; set; }
}