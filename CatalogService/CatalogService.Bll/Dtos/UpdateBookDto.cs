namespace CatalogService.Bll.Dtos;

public class UpdateBookDto
{
    public string Title { get; set; } = null!;
    public int AuthorId { get; set; }
    public int GenreId { get; set; }
}
