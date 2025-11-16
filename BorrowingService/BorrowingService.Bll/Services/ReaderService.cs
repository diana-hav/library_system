using BorrowingService.Bll.Dtos;
using BorrowingService.Dal.Interfaces;
using AutoMapper;

namespace BorrowingService.Bll.Services;

public class ReaderService
{
    private readonly IReaderRepository _repo;
    private readonly IMapper _mapper;

    public ReaderService(IReaderRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ReaderDto>> GetAllAsync()
    {
        var readers = await _repo.GetAllAsync();
        return _mapper.Map<IEnumerable<ReaderDto>>(readers);
    }

    public async Task<ReaderDto?> GetByIdAsync(int id)
    {
        var reader = await _repo.GetByIdAsync(id);
        return _mapper.Map<ReaderDto?>(reader);
    }

    public async Task<int> AddAsync(ReaderDto dto)
    {
        var entity = _mapper.Map<BorrowingService.Domain.Entities.Reader>(dto);
        return await _repo.AddAsync(entity);
    }
}
