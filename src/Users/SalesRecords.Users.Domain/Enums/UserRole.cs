using System.Text.Json.Serialization;

namespace SalesRecords.Users.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]

public enum UserRole
{
    Customer,
    Manager,
    Admin
}
