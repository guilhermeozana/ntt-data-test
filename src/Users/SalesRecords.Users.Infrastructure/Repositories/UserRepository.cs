using Microsoft.EntityFrameworkCore;
using SalesRecords.Users.Domain.Entities;
using SalesRecords.Users.Infrastructure.Context;
using SalesRecords.Shared.SharedKernel.Dtos;
using SalesRecords.Shared.SharedKernel.Extensions;
using SalesRecords.Shared.SharedKernel.Pagination;
using SalesRecords.Users.Application.Common.Interfaces;

namespace SalesRecords.Users.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UsersDbContext _context;

    public UserRepository(UsersDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<User>> GetAllAsync(QueryCriteriaDto queryCriteria)
    {
        var filteredQuery = _context.Users
            .ApplyFiltering(queryCriteria.Filters, queryCriteria.MinValues, queryCriteria.MaxValues);

        var totalCount = await filteredQuery.CountAsync();

        var pagedQuery = filteredQuery
            .ApplyOrdering(queryCriteria.Order)
            .ApplyPagination(queryCriteria.Page, queryCriteria.Size);

        var users = await pagedQuery.ToListAsync();

        return new PagedResult<User>
        {
            Items = users,
            TotalCount = totalCount
        };
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(User user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Users.AnyAsync(u => u.Id == id);
    }
}