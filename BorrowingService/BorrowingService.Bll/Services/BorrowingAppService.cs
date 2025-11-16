using BorrowingService.Domain.Entities;
using BorrowingService.Dal;
using BorrowingService.Bll.Dtos;
using AutoMapper;
using Dapper;
using Npgsql;

namespace BorrowingService.Bll.Services;

public class BorrowingAppService
{
    private readonly string _connectionString;
    private readonly IMapper _mapper;

    public BorrowingAppService(string connectionString, IMapper mapper)
    {
        _connectionString = connectionString;
        _mapper = mapper;
    }

    public async Task<int> IssueBookAsync(int readerId, int bookId)
    {
        using var uow = new UnitOfWork(_connectionString);
        try
        {
            var book = await uow.Books.GetByIdAsync(bookId);
            if (book is null || !book.IsAvailable)
                throw new InvalidOperationException("Book not available");

            var borrowing = new Borrowing
            {
                ReaderId = readerId,
                BookId = bookId,
                BorrowDate = DateTime.UtcNow,
                Status = "active"
            };

            var id = await uow.Borrowings.AddAsync(borrowing);
            await uow.Books.SetAvailabilityAsync(bookId, false);

            await uow.CommitAsync();
            return id;
        }
        catch
        {
            await uow.RollbackAsync();
            throw;
        }
    }

    public async Task ReturnBookAsync(int borrowingId)
    {
        using var uow = new UnitOfWork(_connectionString);
        try
        {
            var borrowing = await uow.Borrowings.GetByIdAsync(borrowingId);
            if (borrowing is null)
                throw new KeyNotFoundException("Borrowing not found");

            await uow.Borrowings.UpdateStatusAsync(borrowingId, "returned");
            await uow.Books.SetAvailabilityAsync(borrowing.BookId, true);

            await uow.CommitAsync();
        }
        catch
        {
            await uow.RollbackAsync();
            throw;
        }
    }

    public async Task<IEnumerable<BorrowingDto>> GetAllAsync()
    {
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();
        
        var sql = @"
            SELECT 
                b.id,
                b.reader_id as ReaderId,
                r.full_name as ReaderName,
                b.book_id as BookId,
                bk.title as BookTitle,
                b.borrow_date as BorrowDate,
                b.return_date as ReturnDate,
                b.status
            FROM borrowings b
            LEFT JOIN readers r ON b.reader_id = r.id
            LEFT JOIN books bk ON b.book_id = bk.id
            ORDER BY b.borrow_date DESC;
        ";
        
        var result = await conn.QueryAsync<BorrowingDto>(sql);
        return result;
    }
}
