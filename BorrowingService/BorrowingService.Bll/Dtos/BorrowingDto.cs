namespace BorrowingService.Bll.Dtos
{

    public class BorrowingDto
    {
        public int Id { get; set; }
        public int ReaderId { get; set; }
        public string ReaderName { get; set; } = string.Empty;
        public int BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public string BookAuthor { get; set; } = string.Empty;
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Status { get; set; } = "active";
    }
}


