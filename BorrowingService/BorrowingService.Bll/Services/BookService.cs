using BorrowingService.Bll.Dtos;
using BorrowingService.Domain.Entities;
using Dapper;
using Npgsql;
using AutoMapper;

namespace BorrowingService.Bll.Services;

public class BookService
{
    private readonly string _connectionString;
    private readonly IMapper _mapper;

    public BookService(string connectionString, IMapper mapper)
    {
        _connectionString = connectionString;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BookDto>> GetAllAsync()
    {
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();
        var sql = "SELECT id, title, author_name, is_available FROM books ORDER BY title;";
        var books = await conn.QueryAsync<Book>(sql);
        return _mapper.Map<IEnumerable<BookDto>>(books);
    }

    public async Task<BookDto?> GetByIdAsync(int id)
    {
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();
        var sql = "SELECT id, title, author_name, is_available FROM books WHERE id=@id;";
        var book = await conn.QuerySingleOrDefaultAsync<Book>(sql, new { id });
        return book != null ? _mapper.Map<BookDto>(book) : null;
    }
}

