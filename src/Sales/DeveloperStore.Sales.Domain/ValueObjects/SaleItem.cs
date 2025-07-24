using DeveloperStore.Shared.SharedKernel.Domain;

namespace DeveloperStore.Sales.Domain.ValueObjects;

public class SaleItem : ValueObject
{
    private decimal _discount;
    private decimal _totalAmount;

    private SaleItem() { } 

    public SaleItem(int productId, int quantity, decimal unitPrice)
    {
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;

        DiscountRate = GetDiscountRate(quantity);
        _discount = quantity * unitPrice * DiscountRate;
        _totalAmount = quantity * unitPrice - _discount;
    }

    public int ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }

    public decimal DiscountRate { get; private set; }

    public decimal Discount => _discount;
    public decimal TotalAmount => _totalAmount;

    private decimal GetDiscountRate(int quantity) => quantity switch
    {
        >= 10 and <= 20 => 0.20m,
        >= 4 and < 10   => 0.10m,
        _               => 0.0m
    };

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ProductId;
        yield return Quantity;
        yield return UnitPrice;
    }
}