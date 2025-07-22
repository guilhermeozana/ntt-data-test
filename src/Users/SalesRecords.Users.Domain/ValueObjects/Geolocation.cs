using SalesRecords.Shared.SharedKernel.Domain;

namespace SalesRecords.Users.Domain.ValueObjects;

public class Geolocation : ValueObject
{
    public string Lat { get; }
    public string Long { get; }

    public Geolocation(string lat, string @long)
    {
        Lat = lat;
        Long = @long;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Lat;
        yield return Long;
    }
}