namespace CatalogService.Bll.Dtos;

public class QueryParametersDto
{
    public int Page { get; set; }        // Підставляється з фронту
    public int PageSize { get; set; }    // Підставляється з фронту

    public string? Title { get; set; }   // Фільтр за назвою
    public int? AuthorId { get; set; }   // Фільтр за автором
    public int? GenreId { get; set; }    // Фільтр за жанром

    public string? SortBy { get; set; }  // Напр., "title", "author", "genre"
    public string? SortDir { get; set; } // "asc" або "desc"
}
