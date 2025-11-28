using AutoMapper;
using CatalogService.Domain.Entities;
using CatalogService.Bll.Dtos;

namespace CatalogService.Bll.Mapping;

public class CatalogProfile : Profile
{
    public CatalogProfile()
    {
        CreateMap<Author, AuthorDto>().ReverseMap();
        CreateMap<Genre, GenreDto>().ReverseMap();
        CreateMap<Book, BookDto>()
            .ForMember(d => d.AuthorName, opt => opt.MapFrom(s => s.Author.Name))
            .ForMember(d => d.GenreName, opt => opt.MapFrom(s => s.Genre.Name))
            .ReverseMap();
        CreateMap<CreateBookDto, Book>();
        CreateMap<UpdateBookDto, Book>();
    }
}
