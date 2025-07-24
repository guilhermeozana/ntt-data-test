using DeveloperStore.Shared.SharedKernel.Domain;

namespace DeveloperStore.Carts.Domain.ValueObjects;

public class CartItem : ValueObject
{
    public int ProductId { get; }
    public int Quantity { get; }

    public CartItem(int productId, int quantity)
    {
        ProductId = productId;
        Quantity = quantity;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ProductId;
        yield return Quantity;
    }
}