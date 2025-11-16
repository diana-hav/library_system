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

    public async Task<int> CreateAsync(AuthorDto dto)
    {
        var entity = _mapper.Map<Author>(dto);
        await _uow.Authors.AddAsync(entity);
        await _uow.SaveAsync();
        return entity.Id;
    }
}
