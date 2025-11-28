using CatalogService.Dal.Repositories;
using CatalogService.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace CatalogService.Dal
{
    public class UnitOfWork : IDisposable
    {
        private readonly CatalogDbContext _context;

        public AuthorRepository Authors { get; }
        public GenreRepository Genres { get; }
        public BookRepository Books { get; }

        public UnitOfWork(CatalogDbContext context)
        {
            _context = context;

            Authors = new AuthorRepository(context);
            Genres = new GenreRepository(context);
            Books = new BookRepository(context);
        }

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}

