using System.Text.Json.Serialization;

namespace DeveloperStore.Users.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]

public enum UserRole
{
    Customer,
    Manager,
    Admin
}
