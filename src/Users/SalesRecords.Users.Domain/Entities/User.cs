using SalesRecords.Shared.SharedKernel.Domain;
using SalesRecords.Users.Domain.Enums;
using SalesRecords.Users.Domain.ValueObjects;

namespace SalesRecords.Users.Domain.Entities;

public class User : AggregateRoot<int>
{
    public string Email { get; private set; }
    public string Username { get; private set; }
    public string Password { get; private set; }
    public Name Name { get; private set; }
    public Address Address { get; private set; }
    public string Phone { get; private set; }
    public UserStatus Status { get; private set; }
    public UserRole Role { get; private set; }

    private User() { }

    public User(
        string email,
        string username,
        string password,
        Name name,
        Address address,
        string phone,
        UserStatus status,
        UserRole role)
    {
        Email = email;
        Username = username;
        Password = password;
        Name = name;
        Address = address;
        Phone = phone;
        Status = status;
        Role = role;
    }

    public static User Create(
        string email,
        string username,
        string password,
        Name name,
        Address address,
        string phone,
        UserStatus status,
        UserRole role)
    {
        return new User(email, username, password, name, address, phone, status, role);
    }

    public void Update(
        string email,
        string username,
        string password,
        Name name,
        Address address,
        string phone,
        UserStatus status,
        UserRole role)
    {
        Email = email;
        Username = username;
        Password = password;
        Name = name;
        Address = address;
        Phone = phone;
        Status = status;
        Role = role;
    }
}