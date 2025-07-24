using ErrorOr;
using DeveloperStore.Sales.Domain.Events;
using DeveloperStore.Sales.Domain.ValueObjects;
using DeveloperStore.Shared.SharedKernel.Domain;

namespace DeveloperStore.Sales.Domain.Entities;

public class Sale : AggregateRoot<int>
{
    public string SaleNumber { get; private set; }
    public DateTime Date { get; private set; }
    public int CustomerId { get; private set; }
    public int BranchId { get; private set; }

    private readonly List<SaleItem> _products = new();

    public IReadOnlyCollection<SaleItem> Products => _products.AsReadOnly();

    public bool IsCancelled { get; private set; }
    public decimal TotalAmount => Products.Sum(i => i.TotalAmount);

    public static Sale Create(string saleNumber, DateTime date, int customerId, int branchId)
    {
        var sale = new Sale
        {
            SaleNumber = saleNumber,
            Date = date,
            CustomerId = customerId,
            BranchId = branchId
        };

        sale.AddDomainEvent(new SaleCreatedEvent(sale.Id, sale.SaleNumber, sale.Date));
        return sale;
    }
    
    public ErrorOr<Success> AddItem(int productId, int quantity, decimal unitPrice)
    {
        if (quantity > 20)
            return Error.Validation("SaleItem.TooManyUnits","Cannot purchase more than 20 identical items.");

        _products.RemoveAll(i => i.ProductId == productId);

        var newItem = new SaleItem(productId, quantity, unitPrice);
        _products.Add(newItem);

        return Result.Success;
    }
    
    public void ClearItems()
    {
        _products.Clear();
    }
    
    public void Update(DateTime newDate, int newCustomerId, int newBranchId)
    {
        Date = newDate;
        CustomerId = newCustomerId;
        BranchId = newBranchId;

        AddDomainEvent(new SaleModifiedEvent(Id, DateTime.UtcNow));
    }
    
    public void Cancel()
    {
        if (IsCancelled) return;

        IsCancelled = true;
        AddDomainEvent(new SaleCancelledEvent(Id, DateTime.UtcNow));

        foreach (var item in Products)
        {
            AddDomainEvent(new ItemCancelledEvent(Id, item.ProductId, DateTime.UtcNow));
        }
    }
}