using DeveloperStore.Carts.Domain.ValueObjects;
using DeveloperStore.Shared.SharedKernel.Domain;
using ErrorOr;

namespace DeveloperStore.Carts.Domain.Entities;

public class Cart : AggregateRoot<int>
{
    public int UserId { get; private set; }
    public DateTime Date { get; private set; }
    public bool IsCancelled { get; private set; }

    private readonly List<CartItem> _products = new();
    public IReadOnlyCollection<CartItem> Products => _products.AsReadOnly();

    private Cart() { }

    public Cart(int userId, DateTime date)
    {
        UserId = userId;
        Date = date;
    }

    public static Cart Create(int userId, DateTime? date = null)
    {
        return new Cart(userId, date ?? DateTime.UtcNow);
    }

    public void Update(int userId, DateTime date)
    {
        UserId = userId;
        Date = date;
    }

    public ErrorOr<Success> AddItem(int productId, int quantity)
    {
        if (quantity > 20)
            return Error.Validation("CartItem.TooManyUnits", "Cannot purchase more than 20 identical items.");

        var item = new CartItem(productId, quantity);
        _products.Add(item);

        return Result.Success;
    }
    
    public void ClearItems()
    {
        _products.Clear();
    }

}