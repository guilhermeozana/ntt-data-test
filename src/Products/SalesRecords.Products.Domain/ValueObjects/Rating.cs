namespace SalesRecords.Products.Domain.ValueObjects;

public class Rating
{
    public Rating(double rate, int count)
    {
        Rate = rate;
        Count = count;
    }

    public double Rate { get; set; }
    public int Count { get; set; }
}