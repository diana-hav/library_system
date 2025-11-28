namespace CatalogService.Bll.Dtos;

public class BookDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public int AuthorId { get; set; }
    public string? AuthorName { get; set; }
    public int GenreId { get; set; }
    public string? GenreName { get; set; }
}
