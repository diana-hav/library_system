using Npgsql;

namespace BorrowingService.Dal.Interfaces
{
    public interface ITransactionalRepository
    {
        void SetTransaction(NpgsqlTransaction transaction);
    }
}
