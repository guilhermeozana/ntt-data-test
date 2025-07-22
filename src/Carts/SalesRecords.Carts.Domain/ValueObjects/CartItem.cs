namespace SalesRecords.Carts.Domain.ValueObjects;

public class CartItem
{
    public int ProductId { get; private set; }
    public int Quantity { get; private set; }
    public bool IsCancelled { get; private set; }

    public CartItem(int productId, int quantity)
    {
        ProductId = productId;
        Quantity = quantity;
        IsCancelled = false;
    }

    public void IncreaseQuantity(int quantity)
    {
        Quantity += quantity;
    }

    public void Cancel()
    {
        IsCancelled = true;
    }
}