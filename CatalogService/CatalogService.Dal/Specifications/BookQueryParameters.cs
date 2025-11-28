namespace CatalogService.Dal.Specifications;

public class QueryParameters
{
    public int Page { get; set; }
    public int PageSize { get; set; }

    public string? Title { get; set; }
    public int? AuthorId { get; set; }
    public int? GenreId { get; set; }

    public string? SortBy { get; set; }
    public string? SortDir { get; set; }
}
