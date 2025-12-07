using TeknoRoma.Domain.Entities;

namespace TeknoRoma.Application.Interfaces.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
    Task<IEnumerable<Category>> GetActiveCategoriesAsync();
    Task<Category?> GetCategoryWithProductsAsync(int id);
}
