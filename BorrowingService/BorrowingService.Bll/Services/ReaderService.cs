using AutoMapper;
using BorrowingService.Bll.Dtos;
using BorrowingService.Dal.Interfaces;
using BorrowingService.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BorrowingService.Bll.Services
{
    public class ReaderService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ReaderService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReaderDto>> GetAllAsync()
        {
            var readers = await _uow.Readers.GetAllAsync();
            return _mapper.Map<IEnumerable<ReaderDto>>(readers);
        }

        public async Task<ReaderDto?> GetByIdAsync(int id)
        {
            var reader = await _uow.Readers.GetByIdAsync(id);
            return _mapper.Map<ReaderDto?>(reader);
        }

        public async Task<int> AddAsync(ReaderDto dto)
        {
            await _uow.BeginTransactionAsync();

            var entity = _mapper.Map<Reader>(dto);
            var id = await _uow.Readers.AddAsync(entity);

            await _uow.CommitAsync();
            return id;
        }
    }
}
