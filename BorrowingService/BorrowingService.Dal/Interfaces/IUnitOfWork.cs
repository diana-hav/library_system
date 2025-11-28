using System.Threading.Tasks;

namespace BorrowingService.Dal.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBookRepository Books { get; }
        IBorrowingRepository Borrowings { get; }
        IReaderRepository Readers { get; }

        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
