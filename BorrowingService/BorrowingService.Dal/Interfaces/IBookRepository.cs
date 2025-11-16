using BorrowingService.Domain.Entities;

namespace BorrowingService.Dal.Interfaces;

public interface IBookRepository
{
    Task<Book?> GetByIdAsync(int id);
    Task<bool> SetAvailabilityAsync(int id, bool available);
    Task<IEnumerable<Book>> GetAllAsync();
}
