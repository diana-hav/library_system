using BorrowingService.Domain.Entities;

namespace BorrowingService.Dal.Interfaces;

public interface IReaderRepository
{
    Task<Reader?> GetByIdAsync(int id);
    Task<IEnumerable<Reader>> GetAllAsync();
    Task<int> AddAsync(Reader reader);
}
