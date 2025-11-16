using BorrowingService.Domain.Entities;
using BorrowingService.Dal.Interfaces;
using Npgsql;

namespace BorrowingService.Dal.Repositories;

public class ReaderRepository : IReaderRepository
{
    private readonly string _connectionString;
    public ReaderRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<int> AddAsync(Reader reader)
    {
        const string sql = @"INSERT INTO readers (full_name, email)
                             VALUES (@full_name, @Email)
                             RETURNING id;";
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@full_name", reader.FullName);
        cmd.Parameters.AddWithValue("@Email", reader.Email);
        var id = (int)await cmd.ExecuteScalarAsync();
        return id;
    }

    public async Task<IEnumerable<Reader>> GetAllAsync()
    {
        var readers = new List<Reader>();
        const string sql = "SELECT id, full_name, email, created_at FROM readers;";
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();
        await using var cmd = new NpgsqlCommand(sql, conn);
        await using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            readers.Add(new Reader
            {
                Id = reader.GetInt32(0),
                FullName = reader.GetString(1),
                Email = reader.GetString(2),
                CreatedAt = reader.GetDateTime(3)
            });
        }
        return readers;
    }

    public async Task<Reader?> GetByIdAsync(int id)
    {
        const string sql = "SELECT id, full_name, email, created_at FROM readers WHERE id=@id;";
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", id);
        await using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Reader
            {
                Id = reader.GetInt32(0),
                FullName = reader.GetString(1),
                Email = reader.GetString(2),
                CreatedAt = reader.GetDateTime(3)
            };
        }
        return null;
    }
}
