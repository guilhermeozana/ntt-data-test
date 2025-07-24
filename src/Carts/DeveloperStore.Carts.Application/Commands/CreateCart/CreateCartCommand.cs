using ErrorOr;
using MediatR;
using DeveloperStore.Carts.Contracts.Dtos;

namespace DeveloperStore.Carts.Application.Commands.CreateCart;

public class CreateCartCommand : IRequest<ErrorOr<CartDto>>
{
    public int UserId { get; set; }
    public string Date { get; set; }
    public List<CartItemDto> Products { get; set; } = new();
}