using System;
using System.Threading.Tasks;
using BorrowingService.Dal.Interfaces;
using Npgsql;

namespace BorrowingService.Dal
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly NpgsqlConnection _connection;
        private NpgsqlTransaction? _transaction;

        public IBookRepository Books { get; }
        public IBorrowingRepository Borrowings { get; }
        public IReaderRepository Readers { get; }

        public UnitOfWork(
            NpgsqlConnection connection,
            IBookRepository books,
            IBorrowingRepository borrowings,
            IReaderRepository readers)
        {
            _connection = connection;
            Books = books;
            Borrowings = borrowings;
            Readers = readers;
        }

        public async Task BeginTransactionAsync()
        {
            if (_connection.State != System.Data.ConnectionState.Open)
                await _connection.OpenAsync();

            _transaction = await _connection.BeginTransactionAsync();

            if (Books is ITransactionalRepository tb) tb.SetTransaction(_transaction);
            if (Borrowings is ITransactionalRepository tbr) tbr.SetTransaction(_transaction);
            if (Readers is ITransactionalRepository tr) tr.SetTransaction(_transaction);
        }

        public async Task CommitAsync()
        {
            if (_transaction == null) throw new InvalidOperationException("Transaction not started");
            await _transaction.CommitAsync();
            await _connection.CloseAsync();
            _transaction.Dispose();
            _transaction = null;
        }

        public async Task RollbackAsync()
        {
            if (_transaction == null) return;
            await _transaction.RollbackAsync();
            await _connection.CloseAsync();
            _transaction.Dispose();
            _transaction = null;
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _connection?.Dispose();
        }
    }
}
