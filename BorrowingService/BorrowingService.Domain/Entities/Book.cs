namespace BorrowingService.Domain.Entities;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string AuthorName { get; set; } = null!;
    public bool IsAvailable { get; set; }
}
