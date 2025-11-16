using CatalogService.Dal.Repositories;
using CatalogService.Domain.Entities;

namespace CatalogService.Dal;

public class UnitOfWork : IDisposable
{
    private readonly CatalogDbContext _context;
    public GenericRepository<Book> Books { get; }
    public GenericRepository<Author> Authors { get; }
    public GenericRepository<Genre> Genres { get; }

    public UnitOfWork(CatalogDbContext context)
    {
        _context = context;
        Books = new GenericRepository<Book>(context);
        Authors = new GenericRepository<Author>(context);
        Genres = new GenericRepository<Genre>(context);
    }

    public async Task<int> SaveAsync() => await _context.SaveChangesAsync();

    public void Dispose() => _context.Dispose();
}
