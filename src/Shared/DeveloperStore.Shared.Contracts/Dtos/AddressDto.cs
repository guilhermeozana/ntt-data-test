namespace DeveloperStore.Shared.Contracts.Dtos;

public class AddressDto
{
    public string City { get; set; } = default!;
    public string Street { get; set; } = default!;
    public int Number { get; set; }
    public string Zipcode { get; set; } = default!;
    public GeolocationDto Geolocation { get; set; } = new();
}