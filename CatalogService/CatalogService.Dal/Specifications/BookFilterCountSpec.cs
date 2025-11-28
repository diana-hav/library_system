using Ardalis.Specification;
using CatalogService.Domain.Entities;

namespace CatalogService.Dal.Specifications
{
    public class BookFilterCountSpec : Specification<Book>
    {
        public BookFilterCountSpec(QueryParameters p)
        {
            if (!string.IsNullOrWhiteSpace(p.Title))
                Query.Where(b => b.Title.ToLower().Contains(p.Title.ToLower()));

            if (p.AuthorId.HasValue)
                Query.Where(b => b.AuthorId == p.AuthorId.Value);

            if (p.GenreId.HasValue)
                Query.Where(b => b.GenreId == p.GenreId.Value);
        }
    }
}
