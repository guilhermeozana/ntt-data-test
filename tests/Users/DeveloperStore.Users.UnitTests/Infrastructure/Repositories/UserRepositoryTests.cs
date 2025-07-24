using DeveloperStore.Shared.SharedKernel.Dtos;
using DeveloperStore.Users.Domain.Entities;
using DeveloperStore.Users.Domain.Enums;
using DeveloperStore.Users.Domain.ValueObjects;
using DeveloperStore.Users.Infrastructure.Context;
using DeveloperStore.Users.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Users.UnitTests.Infrastructure.Repositories;

public class UserRepositoryTests
{
    private static User CreateUser(string username = "admin", string email = "admin@example.com")
    {
        var name = new Name("Junior", "Silva");
        var geo = new Geolocation("22.9068", "-43.1729");
        var address = new Address("Rio de Janeiro", "Rua 1", 123, "12345-678", geo);

        return User.Create(
            email,
            username,
            "securePassword",
            name,
            address,
            "11999999999",
            UserStatus.Active,
            UserRole.Admin
        );
    }

    private UserRepository GetRepository(out UsersDbContext context)
    {
        var options = new DbContextOptionsBuilder<UsersDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        context = new UsersDbContext(options);
        context.Database.EnsureCreated();
        return new UserRepository(context);
    }

    [Fact]
    public async Task AddAsync_ShouldPersistUser()
    {
        var repo = GetRepository(out var context);
        var user = CreateUser();

        await repo.AddAsync(user);

        context.Users.Should().ContainSingle();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnUser()
    {
        var repo = GetRepository(out var context);
        var user = CreateUser();
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        var result = await repo.GetByIdAsync(user.Id);

        result.Should().NotBeNull();
        result!.Username.Should().Be(user.Username);
    }

    [Fact]
    public async Task GetByUsernameAsync_ShouldFindUserByUsername()
    {
        var repo = GetRepository(out var context);
        var user = CreateUser("uniqueuser");
        await repo.AddAsync(user);

        var found = await repo.GetByUsernameAsync("uniqueuser");

        found.Should().NotBeNull();
        found!.Email.Should().Be(user.Email);
    }

    [Fact]
    public async Task UpdateAsync_ShouldApplyChanges()
    {
        var repo = GetRepository(out var context);
        var user = CreateUser();
        await repo.AddAsync(user);

        var newName = new Name("João", "Oliveira");
        var geo = new Geolocation("22.9100", "-43.1700");
        var newAddress = new Address("Rio de Janeiro", "Rua 1", 123, "12345-678", geo);

        user.Update("joao@example.com", "joao", "newPass", newName, newAddress, "21999999999", UserStatus.Inactive, UserRole.Customer);
        await repo.UpdateAsync(user);

        var updated = await repo.GetByIdAsync(user.Id);
        updated!.Username.Should().Be("joao");
        updated.Status.Should().Be(UserStatus.Inactive);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveUser()
    {
        var repo = GetRepository(out var context);
        var user = CreateUser();
        await repo.AddAsync(user);

        var result = await repo.DeleteAsync(user);

        result.Should().BeTrue();
        context.Users.Should().BeEmpty();
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrueIfUserExists()
    {
        var repo = GetRepository(out var context);
        var user = CreateUser();
        await repo.AddAsync(user);

        var exists = await repo.ExistsAsync(user.Id);

        exists.Should().BeTrue();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnPagedUsers()
    {
        var repo = GetRepository(out var context);

        for (int i = 0; i < 10; i++)
        {
            var user = CreateUser($"user{i}", $"user{i}@test.com");
            await repo.AddAsync(user);
        }

        var criteria = new QueryCriteriaDto(page: 1, size: 5, order: null, filters: null, minValues: null, maxValues: null);
        var result = await repo.GetAllAsync(criteria);

        result.Items.Should().HaveCount(5);
        result.TotalCount.Should().Be(10);
    }
}