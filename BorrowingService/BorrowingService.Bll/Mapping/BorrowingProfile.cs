using AutoMapper;
using BorrowingService.Domain.Entities;
using BorrowingService.Bll.Dtos;

namespace BorrowingService.Bll.Mapping;

public class BorrowingProfile : Profile
{
    public BorrowingProfile()
    {
        CreateMap<Book, BookDto>()
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.AuthorName));

        CreateMap<Reader, ReaderDto>().ReverseMap();

        CreateMap<Borrowing, BorrowingDto>().ReverseMap();
    }
}
