using AutoMapper;
using CatalogService.Bll.Dtos;
using CatalogService.Dal;
using CatalogService.Domain.Entities;

namespace CatalogService.Bll.Services;

public class GenreService
{
    private readonly UnitOfWork _uow;
    private readonly IMapper _mapper;

    public GenreService(UnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GenreDto>> GetAllAsync()
    {
        var genres = await _uow.Genres.GetAllAsync();
        return _mapper.Map<IEnumerable<GenreDto>>(genres);
    }

    public async Task<GenreDto> GetByIdAsync(int id)
    {
        var genre = await _uow.Genres.GetByIdAsync(id);
        if (genre == null)
            throw new KeyNotFoundException($"Genre with id {id} not found");

        return _mapper.Map<GenreDto>(genre);
    }

    public async Task<GenreDto> CreateAsync(GenreDto dto)
    {
        if (await _uow.Genres.ExistsByNameAsync(dto.Name))
            throw new InvalidOperationException("Genre with the same name already exists");

        var entity = _mapper.Map<Genre>(dto);
        await _uow.Genres.AddAsync(entity);
        await _uow.SaveChangesAsync();

        return _mapper.Map<GenreDto>(entity);
    }

    public async Task UpdateAsync(int id, GenreDto dto)
    {
        var genre = await _uow.Genres.GetByIdAsync(id);
        if (genre == null)
            throw new KeyNotFoundException($"Genre with id {id} not found");

        if (await _uow.Genres.ExistsByNameAsync(dto.Name) &&
            genre.Name.Trim().ToLower() != dto.Name.Trim().ToLower())
        {
            throw new InvalidOperationException("Another genre with the same name already exists");
        }

        genre.Name = dto.Name;

        _uow.Genres.Update(genre);
        await _uow.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var genre = await _uow.Genres.GetByIdAsync(id);
        if (genre == null)
            throw new KeyNotFoundException($"Genre with id {id} not found");

        _uow.Genres.Delete(genre);
        await _uow.SaveChangesAsync();
    }
}
    