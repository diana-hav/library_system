namespace CatalogService.Domain.Entities;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public int AuthorId { get; set; }
    public int GenreId { get; set; }

    public Author Author { get; set; } = null!;
    public Genre Genre { get; set; } = null!;
}
