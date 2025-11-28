using BorrowingService.Domain.Entities;

namespace BorrowingService.Dal.Interfaces
{
    public interface IBookRepository
    {
        Task<Book?> GetByIdAsync(int id);
        Task<Book?> GetByTitleAndAuthorAsync(string title, string author);
        Task<int> CreateBookAsync(string title, string author);
        Task<int> SetAvailabilityAsync(int id, bool available); // <-- повертає affected rows
        Task<IEnumerable<Book>> GetAllAsync();
    }
}
