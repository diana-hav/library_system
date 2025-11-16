using BorrowingService.Domain.Entities;

namespace BorrowingService.Dal.Interfaces;

public interface IBorrowingRepository
{
    Task<IEnumerable<Borrowing>> GetAllAsync();
    Task<Borrowing?> GetByIdAsync(int id);
    Task<int> AddAsync(Borrowing borrowing);
    Task UpdateStatusAsync(int id, string status);
}
