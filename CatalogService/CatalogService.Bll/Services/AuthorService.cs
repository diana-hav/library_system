using AutoMapper;
using CatalogService.Bll.Dtos;
using CatalogService.Dal;
using CatalogService.Domain.Entities;

namespace CatalogService.Bll.Services;

public class AuthorService
{
    private readonly UnitOfWork _uow;
    private readonly IMapper _mapper;

    public AuthorService(UnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AuthorDto>> GetAllAsync()
    {
        var authors = await _uow.Authors.GetAllAsync();
        return _mapper.Map<IEnumerable<AuthorDto>>(authors);
    }

    public async Task<AuthorDto> GetByIdAsync(int id)
    {
        var author = await _uow.Authors.GetByIdAsync(id);
        if (author == null)
            throw new KeyNotFoundException($"Author with id {id} not found");

        return _mapper.Map<AuthorDto>(author);
    }

    public async Task<AuthorDto> CreateAsync(AuthorDto dto)
    {
        if (await _uow.Authors.ExistsByNameAsync(dto.Name))
            throw new InvalidOperationException("Author with the same name already exists");

        var entity = _mapper.Map<Author>(dto);
        await _uow.Authors.AddAsync(entity);
        await _uow.SaveChangesAsync();

        return _mapper.Map<AuthorDto>(entity);
    }

    public async Task UpdateAsync(int id, AuthorDto dto)
    {
        var author = await _uow.Authors.GetByIdAsync(id);
        if (author == null)
            throw new KeyNotFoundException($"Author with id {id} not found");

        if (await _uow.Authors.ExistsByNameAsync(dto.Name) &&
            author.Name.Trim().ToLower() != dto.Name.Trim().ToLower())
        {
            throw new InvalidOperationException("Another author with the same name already exists");
        }

        author.Name = dto.Name;

        _uow.Authors.Update(author);
        await _uow.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var author = await _uow.Authors.GetByIdAsync(id);
        if (author == null)
            throw new KeyNotFoundException($"Author with id {id} not found");

        _uow.Authors.Delete(author);
        await _uow.SaveChangesAsync();
    }
}
