using Microsoft.EntityFrameworkCore;
using TeknoRoma.Application.Interfaces.Repositories;
using TeknoRoma.Domain.Entities;
using TeknoRoma.Infrastructure.Data;

namespace TeknoRoma.Infrastructure.Repositories;

public class UserRoleRepository : Repository<UserRole>, IUserRoleRepository
{
    public UserRoleRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<UserRole>> GetByUserIdAsync(int userId)
    {
        return await _dbSet
            .Where(ur => ur.UserId == userId)
            .Include(ur => ur.Role)
            .ToListAsync();
    }

    public async Task<IEnumerable<UserRole>> GetByRoleIdAsync(int roleId)
    {
        return await _dbSet
            .Where(ur => ur.RoleId == roleId)
            .Include(ur => ur.User)
            .ToListAsync();
    }

    public async Task DeleteByUserIdAsync(int userId)
    {
        var userRoles = await _dbSet.Where(ur => ur.UserId == userId).ToListAsync();
        _dbSet.RemoveRange(userRoles);
    }

    public async Task<bool> UserHasRoleAsync(int userId, int roleId)
    {
        return await _dbSet.AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
    }
}
