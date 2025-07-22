using SalesRecords.Shared.SharedKernel.Domain;

namespace SalesRecords.Users.Domain.ValueObjects;

public class Name : ValueObject
{
    public string Firstname { get; }
    public string Lastname { get; }

    public Name(string firstname, string lastname)
    {
        Firstname = firstname;
        Lastname = lastname;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Firstname;
        yield return Lastname;
    }
}