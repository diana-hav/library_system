using CatalogService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Dal.Repositories;

public class BookRepository : GenericRepository<Book>
{
    public BookRepository(CatalogDbContext context) : base(context) { }

    public async Task<List<Book>> GetBooksWithAuthorGenreAsync()
    {
        return await _dbSet
            .Include(b => b.Author)
            .Include(b => b.Genre)
            .ToListAsync();
    }

    public async Task<Book?> GetByIdWithRelationsAsync(int id)
    {
        return await _dbSet
            .Include(b => b.Author)
            .Include(b => b.Genre)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<bool> ExistsDuplicateAsync(string title, int authorId, int genreId)
    {
        return await _dbSet.AnyAsync(b =>
            b.Title == title &&
            b.AuthorId == authorId &&
            b.GenreId == genreId);
    }

    public IQueryable<Book> QueryBooks()
    {
        return _dbSet
            .Include(b => b.Author)
            .Include(b => b.Genre)
            .AsQueryable();
    }

}
