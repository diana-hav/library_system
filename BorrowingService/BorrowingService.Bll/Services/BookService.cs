using AutoMapper;
using BorrowingService.Bll.Dtos;
using BorrowingService.Dal.Interfaces;
using BorrowingService.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BorrowingService.Bll.Services
{
    public class BookService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public BookService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookDto>> GetAllAsync()
        {
            var books = await _uow.Books.GetAllAsync();
            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public async Task<BookDto?> GetByIdAsync(int id)
        {
            var book = await _uow.Books.GetByIdAsync(id);
            return _mapper.Map<BookDto?>(book);
        }

        public async Task<int> CreateAsync(string title, string author)
        {
            await _uow.BeginTransactionAsync();
            var id = await _uow.Books.CreateBookAsync(title, author);
            await _uow.CommitAsync();
            return id;
        }

        public async Task<bool> SetAvailabilityAsync(int id, bool available)
        {
            await _uow.BeginTransactionAsync();
            var affected = await _uow.Books.SetAvailabilityAsync(id, available);

            if (affected > 0)
            {
                await _uow.CommitAsync();
                return true;
            }

            await _uow.RollbackAsync();
            return false;
        }
    }
}
