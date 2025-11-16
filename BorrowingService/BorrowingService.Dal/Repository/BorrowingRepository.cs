using BorrowingService.Domain.Entities;
using BorrowingService.Dal.Interfaces;
using Dapper;
using Npgsql;

namespace BorrowingService.Dal.Repositories;

public class BorrowingRepository : IBorrowingRepository
{
    private readonly NpgsqlConnection _connection;
    private readonly NpgsqlTransaction? _transaction;

    public BorrowingRepository(NpgsqlConnection connection, NpgsqlTransaction? transaction = null)
    {
        _connection = connection;
        _transaction = transaction;
    }

    public async Task<IEnumerable<Borrowing>> GetAllAsync()
    {
        var sql = "SELECT id, reader_id, book_id, borrow_date, return_date, status FROM borrowings;";
        return await _connection.QueryAsync<Borrowing>(sql, transaction: _transaction);
    }

    public async Task<Borrowing?> GetByIdAsync(int id)
    {
        var sql = "SELECT id, reader_id, book_id, borrow_date, return_date, status FROM borrowings WHERE id=@id;";
        return await _connection.QuerySingleOrDefaultAsync<Borrowing>(sql, new { id }, _transaction);
    }

    public async Task<int> AddAsync(Borrowing borrowing)
    {
        var sql = @"INSERT INTO borrowings (reader_id, book_id, borrow_date, status)
                    VALUES (@ReaderId, @BookId, @BorrowDate, @Status)
                    RETURNING id;";
        return await _connection.ExecuteScalarAsync<int>(sql, borrowing, _transaction);
    }

    public async Task UpdateStatusAsync(int id, string status)
    {
        var sql = @"UPDATE borrowings SET status=@status, return_date=CASE WHEN @status='returned' THEN NOW() ELSE return_date END
                    WHERE id=@id;";
        await _connection.ExecuteAsync(sql, new { id, status }, _transaction);
    }
}
