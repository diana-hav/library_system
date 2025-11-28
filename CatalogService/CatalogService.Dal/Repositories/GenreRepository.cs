using CatalogService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Dal.Repositories;

public class GenreRepository : GenericRepository<Genre>
{
    public GenreRepository(CatalogDbContext context) : base(context) { }

    public async Task<bool> ExistsByNameAsync(string name)
    {
        return await _dbSet.AnyAsync(g => g.Name.ToLower().Trim() == name.ToLower().Trim());
    }

    public async Task<Genre?> GetGenreWithBooksAsync(int id)
    {
        return await _dbSet
            .Include(g => g.Books)
            .FirstOrDefaultAsync(g => g.Id == id);
    }
}
