using SalesRecords.Shared.SharedKernel.Domain;

namespace SalesRecords.Users.Domain.ValueObjects;

public class Address : ValueObject
{
    public string City { get; }
    public string Street { get; }
    public int Number { get; }
    public string Zipcode { get; }
    public Geolocation Geolocation { get; }

    public Address(string city, string street, int number, string zipcode, Geolocation geolocation)
    {
        City = city;
        Street = street;
        Number = number;
        Zipcode = zipcode;
        Geolocation = geolocation;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return City;
        yield return Street;
        yield return Number;
        yield return Zipcode;
        yield return Geolocation;
    }
}