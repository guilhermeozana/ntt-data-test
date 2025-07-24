namespace DeveloperStore.Products.Domain.ValueObjects;

public class Rating
{
    public Rating(double rate, int count)
    {
        Rate = rate;
        Count = count;
    }
    
    private Rating() { }

    public double Rate { get; set; } = 0;
    public int Count { get; set; } = 0;
}