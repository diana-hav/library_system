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

    public async Task<int> CreateAsync(GenreDto dto)
    {
        var entity = _mapper.Map<Genre>(dto);
        await _uow.Genres.AddAsync(entity);
        await _uow.SaveAsync();
        return entity.Id;
    }
}
