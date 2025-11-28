using CatalogService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Dal.Repositories
{
    public class AuthorRepository : GenericRepository<Author>, IRepository<Author>
    {
        public AuthorRepository(CatalogDbContext context) : base(context) { }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _dbSet.AnyAsync(a => a.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public async Task<Author?> GetAuthorWithBooksAsync(int id)
        {
            return await _dbSet
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
