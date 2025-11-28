using Ardalis.Specification;
using CatalogService.Domain.Entities;

namespace CatalogService.Dal.Specifications
{
    public class BookFilterSortPaginatedSpec : Specification<Book>
    {
        public BookFilterSortPaginatedSpec(QueryParameters p)
        {
            Query.Include(b => b.Author)
                 .Include(b => b.Genre);

            if (!string.IsNullOrWhiteSpace(p.Title))
                Query.Where(b => b.Title.ToLower().Contains(p.Title.ToLower()));

            if (p.AuthorId.HasValue)
                Query.Where(b => b.AuthorId == p.AuthorId.Value);

            if (p.GenreId.HasValue)
                Query.Where(b => b.GenreId == p.GenreId.Value);

            if (!string.IsNullOrWhiteSpace(p.SortBy))
            {
                var sort = p.SortBy.ToLower();
                if (sort == "author")
                    Query.OrderByDynamic(b => b.Author.Name, p.SortDir);
                else if (sort == "genre")
                    Query.OrderByDynamic(b => b.Genre.Name, p.SortDir);
                else
                    Query.OrderByDynamic(b => b.Title, p.SortDir);
            }

            if (p.Page > 0 && p.PageSize > 0)
                Query.Skip((p.Page - 1) * p.PageSize)
                     .Take(p.PageSize);
        }
    }
}
