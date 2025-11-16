using AutoMapper;
using CatalogService.Bll.Dtos;
using CatalogService.Dal;
using CatalogService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Bll.Services;

public class BookService
{
    private readonly CatalogDbContext _context;
    private readonly UnitOfWork _uow;
    private readonly IMapper _mapper;

    public BookService(CatalogDbContext context, UnitOfWork uow, IMapper mapper)
    {
        _context = context;
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BookDto>> GetAllAsync()
    {
        var books = await _context.Books.Include(b => b.Author).Include(b => b.Genre).ToListAsync();
        return _mapper.Map<IEnumerable<BookDto>>(books);
    }

    public async Task<BookDto?> GetByIdAsync(int id)
    {
        var book = await _context.Books
            .Include(b => b.Author)
            .Include(b => b.Genre)
            .FirstOrDefaultAsync(b => b.Id == id);
        return book == null ? null : _mapper.Map<BookDto>(book);
    }

    public async Task<int> CreateAsync(CreateBookDto dto)
    {
        var book = _mapper.Map<Book>(dto);
        await _uow.Books.AddAsync(book);
        await _uow.SaveAsync();
        return book.Id;
    }

    public async Task UpdateAsync(int id, UpdateBookDto dto)
    {
        var book = await _uow.Books.GetByIdAsync(id) ?? throw new KeyNotFoundException("Book not found");
        book.Title = dto.Title;
        book.AuthorId = dto.AuthorId;
        book.GenreId = dto.GenreId;
        _uow.Books.Update(book);
        await _uow.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var book = await _uow.Books.GetByIdAsync(id) ?? throw new KeyNotFoundException("Book not found");
        _uow.Books.Delete(book);
        await _uow.SaveAsync();
    }
}
