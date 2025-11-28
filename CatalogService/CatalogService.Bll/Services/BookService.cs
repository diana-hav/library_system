using AutoMapper;
using CatalogService.Bll.Dtos;
using CatalogService.Dal;
using CatalogService.Dal.Specifications;
using CatalogService.Domain.Entities;

namespace CatalogService.Bll.Services;

public class BookService
{
    private readonly UnitOfWork _uow;
    private readonly IMapper _mapper;

    public BookService(UnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BookDto>> GetAllAsync()
    {
        var books = await _uow.Books.GetBooksWithAuthorGenreAsync();
        return _mapper.Map<IEnumerable<BookDto>>(books);
    }

    public async Task<BookDto> GetByIdAsync(int id)
    {
        var book = await _uow.Books.GetByIdWithRelationsAsync(id);
        if (book == null)
            throw new KeyNotFoundException($"Book with id {id} not found");

        return _mapper.Map<BookDto>(book);
    }

    public async Task<BookDto> CreateAsync(CreateBookDto dto)
    {
        if (await _uow.Books.ExistsDuplicateAsync(dto.Title, dto.AuthorId, dto.GenreId))
            throw new InvalidOperationException("Book with same title, author and genre already exists");

        var book = _mapper.Map<Book>(dto);
        await _uow.Books.AddAsync(book);
        await _uow.SaveChangesAsync();

        return _mapper.Map<BookDto>(book);
    }

    public async Task UpdateAsync(int id, UpdateBookDto dto)
    {
        var book = await _uow.Books.GetByIdAsync(id);
        if (book == null)
            throw new KeyNotFoundException($"Book with id {id} not found");

        if (await _uow.Books.ExistsDuplicateAsync(dto.Title, dto.AuthorId, dto.GenreId)
            && (book.Title != dto.Title || book.AuthorId != dto.AuthorId || book.GenreId != dto.GenreId))
        {
            throw new InvalidOperationException("Another book with same title, author, and genre already exists");
        }

        book.Title = dto.Title;
        book.AuthorId = dto.AuthorId;
        book.GenreId = dto.GenreId;

        _uow.Books.Update(book);
        await _uow.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var book = await _uow.Books.GetByIdAsync(id);
        if (book == null)
            throw new KeyNotFoundException($"Book with id {id} not found");

        _uow.Books.Delete(book);
        await _uow.SaveChangesAsync();
    }

    public async Task<PagedResult<BookDto>> GetPagedAsync(QueryParametersDto parameters)
    {
        var dalParams = new QueryParameters
        {
            Page = parameters.Page,
            PageSize = parameters.PageSize,
            Title = parameters.Title,
            AuthorId = parameters.AuthorId,
            GenreId = parameters.GenreId,
            SortBy = parameters.SortBy,
            SortDir = parameters.SortDir
        };

        var listSpec = new BookFilterSortPaginatedSpec(dalParams);
        var countSpec = new BookFilterCountSpec(dalParams);

        var items = await _uow.Books.ListAsync(listSpec);
        var totalCount = await _uow.Books.CountAsync(countSpec);

        return new PagedResult<BookDto>(
            _mapper.Map<IEnumerable<BookDto>>(items),
            totalCount,
            dalParams.Page,
            dalParams.PageSize
        );
    }

}
