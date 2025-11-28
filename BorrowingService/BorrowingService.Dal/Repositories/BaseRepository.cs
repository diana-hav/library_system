using Npgsql;
using BorrowingService.Dal.Interfaces;

namespace BorrowingService.Dal.Repositories
{
    public abstract class BaseRepository : ITransactionalRepository
    {
        protected readonly NpgsqlConnection Connection;
        protected NpgsqlTransaction? Transaction;

        protected BaseRepository(NpgsqlConnection connection)
        {
            Connection = connection;
        }

        public void SetTransaction(NpgsqlTransaction transaction)
        {
            Transaction = transaction;
        }
    }
}
