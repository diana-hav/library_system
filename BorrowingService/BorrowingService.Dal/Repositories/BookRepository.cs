using BorrowingService.Domain.Entities;
using BorrowingService.Dal.Interfaces;
using Dapper;
using Npgsql;

namespace BorrowingService.Dal.Repositories;

public class BookRepository : IBookRepository
{
    private readonly NpgsqlConnection _connection;
    private readonly NpgsqlTransaction? _transaction;

    public BookRepository(
        NpgsqlConnection connection,
        NpgsqlTransaction? transaction = null)
    {
        _connection = connection;
        _transaction = transaction;
    }

    public Task<Book?> GetByIdAsync(int id)
    {
        var sql = @"SELECT id, title, author_name AS AuthorName, is_available AS IsAvailable 
                    FROM books WHERE id=@id;";
        return _connection.QuerySingleOrDefaultAsync<Book>(sql, new { id }, _transaction);
    }

    public Task<Book?> GetByTitleAndAuthorAsync(string title, string author)
    {
        var sql = @"SELECT id, title, author_name AS AuthorName, is_available AS IsAvailable
                    FROM books
                    WHERE title=@title AND author_name=@author;";

        return _connection.QuerySingleOrDefaultAsync<Book>(sql, new { title, author }, _transaction);
    }

    public Task<IEnumerable<Book>> GetAllAsync()
    {
        var sql = @"SELECT id, title, author_name AS AuthorName, is_available AS IsAvailable 
                    FROM books ORDER BY title;";
        return _connection.QueryAsync<Book>(sql, transaction: _transaction);
    }

    public Task<int> CreateBookAsync(string title, string author)
    {
        var sql = @"INSERT INTO books (title, author_name, is_available)
                    VALUES (@title, @author, TRUE)
                    RETURNING id;";
        return _connection.ExecuteScalarAsync<int>(sql, new { title, author }, _transaction);
    }

    public Task<int> SetAvailabilityAsync(int id, bool available)
    {
        var sql = @"UPDATE books SET is_available=@available WHERE id=@id;";
        return _connection.ExecuteAsync(sql, new { id, available }, _transaction);
    }
}
