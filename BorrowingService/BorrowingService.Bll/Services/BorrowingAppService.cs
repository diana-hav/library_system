using AutoMapper;
using BorrowingService.Bll.Dtos;
using BorrowingService.Dal.Interfaces;
using BorrowingService.Domain.Entities;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace BorrowingService.Bll.Services
{
    public class BorrowingAppService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly HttpClient _http;

        public BorrowingAppService(IUnitOfWork uow, IMapper mapper, HttpClient http)
        {
            _uow = uow;
            _mapper = mapper;
            _http = http;
        }

        private async Task<CatalogBookDto> GetCatalogBookByIdAsync(int catalogBookId)
        {
            var catalogBook = await _http.GetFromJsonAsync<CatalogBookDto>(
                $"http://localhost:5180/api/books/{catalogBookId}");

            if (catalogBook is null)
                throw new InvalidOperationException("Catalog book not found");

            return catalogBook;
        }
        public async Task<int> IssueBookAsync(int readerId, int catalogBookId)
        {
            await _uow.BeginTransactionAsync();

            try
            {
                var catalogBook = await GetCatalogBookByIdAsync(catalogBookId);

                var book = await _uow.Books.GetByTitleAndAuthorAsync(
                    catalogBook.Title,
                    catalogBook.AuthorName
                );

                if (book == null)
                {
                    var newId = await _uow.Books.CreateBookAsync(catalogBook.Title, catalogBook.AuthorName);
                    book = await _uow.Books.GetByIdAsync(newId);
                }

                if (!book!.IsAvailable)
                    throw new InvalidOperationException("Book not available");

                var borrowing = new Borrowing
                {
                    ReaderId = readerId,
                    BookId = book.Id,
                    BorrowDate = DateTime.UtcNow,
                    Status = "active"
                };

                var borrowingId = await _uow.Borrowings.AddAsync(borrowing);

                await _uow.Books.SetAvailabilityAsync(book.Id, false);

                await _uow.CommitAsync();
                return borrowingId;
            }
            catch
            {
                await _uow.RollbackAsync();
                throw;
            }
        }

        public async Task ReturnBookAsync(int borrowingId)
        {
            await _uow.BeginTransactionAsync();

            try
            {
                var bor = await _uow.Borrowings.GetByIdAsync(borrowingId);
                if (bor == null)
                    throw new KeyNotFoundException("Borrowing not found");

                await _uow.Borrowings.UpdateStatusAsync(borrowingId, "returned");
                await _uow.Books.SetAvailabilityAsync(bor.BookId, true);

                await _uow.CommitAsync();
            }
            catch
            {
                await _uow.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<BorrowingDto>> GetAllAsync()
        {
            var list = await _uow.Borrowings.GetAllAsync();
            var result = new List<BorrowingDto>();

            foreach (var b in list)
            {
                var reader = await _uow.Readers.GetByIdAsync(b.ReaderId);
                var book = await _uow.Books.GetByIdAsync(b.BookId);

                var dto = new BorrowingDto
                {
                    Id = b.Id,
                    ReaderId = b.ReaderId,
                    ReaderName = reader?.FullName ?? "Н/Д",
                    BookId = b.BookId,
                    BookTitle = book?.Title ?? "Н/Д",
                    BorrowDate = b.BorrowDate,
                    ReturnDate = b.ReturnDate,
                    Status = b.Status
                };

                result.Add(dto);
            }

            return result;
        }

    }
}
