using SalesRecords.Carts.Domain.Events;
using SalesRecords.Carts.Domain.ValueObjects;
using SalesRecords.Shared.SharedKernel.Domain;

namespace SalesRecords.Carts.Domain.Entities;

public class Cart : AggregateRoot<int>
{
    public int UserId { get; private set; }
    public DateTime Date { get; private set; }
    public bool IsCancelled { get; private set; }

    private readonly List<CartItem> _items = new();
    public IReadOnlyCollection<CartItem> Items => _items.AsReadOnly();

    private Cart() { }

    public Cart(int userId, DateTime date)
    {
        UserId = userId;
        Date = date;
    }

    public static Cart Create(int userId, DateTime? date = null)
    {
        var cart = new Cart(userId, date ?? DateTime.UtcNow);
        cart.AddDomainEvent(new SaleCreatedEvent(cart.Id, cart.Date));
        return cart;
    }

    public void Update(int userId, DateTime date)
    {
        UserId = userId;
        Date = date;
        AddDomainEvent(new SaleModifiedEvent(Id, DateTime.UtcNow));
    }

    public void AddItem(int productId, int quantity)
    {
        var existing = _items.FirstOrDefault(i => i.ProductId == productId);

        if (existing is not null)
        {
            existing.IncreaseQuantity(quantity);
        }
        else
        {
            _items.Add(new CartItem(productId, quantity));
        }
    }

    public void Cancel()
    {
        if (IsCancelled)
            return;

        IsCancelled = true;

        foreach (var item in _items.Where(i => !i.IsCancelled))
        {
            item.Cancel();
            
            AddDomainEvent(new ItemCancelledEvent(Id, item.ProductId, DateTime.UtcNow));
        }
        
        AddDomainEvent(new SaleCancelledEvent(Id, DateTime.UtcNow));
    }

    public void CancelItem(int productId)
    {
        var item = _items.FirstOrDefault(i => i.ProductId == productId);
        if (item is null || item.IsCancelled)
            return;

        item.Cancel();
        AddDomainEvent(new ItemCancelledEvent(Id, productId, DateTime.UtcNow));
    }
}