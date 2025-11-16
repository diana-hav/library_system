using BorrowingService.Domain.Entities;
using BorrowingService.Dal.Interfaces;
using Dapper;
using Npgsql;

namespace BorrowingService.Dal.Repositories;

public class BookRepository : IBookRepository
{
    private readonly NpgsqlConnection _connection;
    private readonly NpgsqlTransaction? _transaction;

    public BookRepository(NpgsqlConnection connection, NpgsqlTransaction? transaction = null)
    {
        _connection = connection;
        _transaction = transaction;
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        var sql = "SELECT id, title, author_name, is_available FROM books WHERE id=@id;";
        return await _connection.QuerySingleOrDefaultAsync<Book>(sql, new { id }, _transaction);
    }

    public async Task<bool> SetAvailabilityAsync(int id, bool available)
    {
        var sql = "UPDATE books SET is_available=@available WHERE id=@id;";
        var affected = await _connection.ExecuteAsync(sql, new { id, available }, _transaction);
        return affected > 0;
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        var sql = "SELECT id, title, author_name, is_available FROM books ORDER BY title;";
        return await _connection.QueryAsync<Book>(sql, transaction: _transaction);
    }
}
