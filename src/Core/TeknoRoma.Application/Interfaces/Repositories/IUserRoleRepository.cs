using TeknoRoma.Domain.Entities;

namespace TeknoRoma.Application.Interfaces.Repositories;

public interface IUserRoleRepository : IRepository<UserRole>
{
    Task<IEnumerable<UserRole>> GetByUserIdAsync(int userId);
    Task<IEnumerable<UserRole>> GetByRoleIdAsync(int roleId);
    Task DeleteByUserIdAsync(int userId);
    Task<bool> UserHasRoleAsync(int userId, int roleId);
}
