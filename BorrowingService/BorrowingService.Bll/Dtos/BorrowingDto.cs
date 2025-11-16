namespace BorrowingService.Bll.Dtos;

public class BorrowingDto
{
    public int Id { get; set; }
    public int ReaderId { get; set; }
    public string ReaderName { get; set; } = null!;
    public int BookId { get; set; }
    public string BookTitle { get; set; } = null!;
    public DateTime BorrowDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public string Status { get; set; } = "active";
}


