using BorrowingService.Dal.Interfaces;
using BorrowingService.Dal.Repositories;
using Npgsql;

namespace BorrowingService.Dal;

public class UnitOfWork : IDisposable
{
    private readonly NpgsqlConnection _connection;
    private NpgsqlTransaction? _transaction;

    public IBookRepository Books { get; private set; }
    public IBorrowingRepository Borrowings { get; private set; }

    public UnitOfWork(string connectionString)
    {
        _connection = new NpgsqlConnection(connectionString);
        _connection.Open();
        _transaction = _connection.BeginTransaction();

        Books = new BookRepository(_connection, _transaction);
        Borrowings = new BorrowingRepository(_connection, _transaction);
    }

    public async Task CommitAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _connection.CloseAsync();
            _transaction.Dispose();
            _transaction = null;
        }
    }

    public async Task RollbackAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _connection.CloseAsync();
            _transaction.Dispose();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _connection.Dispose();
    }
}
