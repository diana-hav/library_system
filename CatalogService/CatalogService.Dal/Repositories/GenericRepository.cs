using Microsoft.EntityFrameworkCore;

namespace CatalogService.Dal.Repositories;

public class GenericRepository<T> where T : class
{
    private readonly CatalogDbContext _context;
    public GenericRepository(CatalogDbContext context) => _context = context;

    public async Task<IEnumerable<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();
    public async Task<T?> GetByIdAsync(int id) => await _context.Set<T>().FindAsync(id);
    public async Task AddAsync(T entity) => await _context.Set<T>().AddAsync(entity);
    public void Update(T entity) => _context.Set<T>().Update(entity);
    public void Delete(T entity) => _context.Set<T>().Remove(entity);
}
