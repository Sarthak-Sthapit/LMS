using AutoMapper;
using RestAPI.Models;
using RestAPI.DTOs;
using RestAPI.Application.Commands;
using RestAPI.Application.Queries;

namespace RestAPI.Mappers
{
    public class BookMappingProfile : Profile
    {
        public BookMappingProfile()
        {
            // Map for Create
            CreateMap<CreateBookDto, Book>();

            // Map for Update
            CreateMap<UpdateBookDto, Book>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // Entity → Result mappings
            CreateMap<Book, CreateBookResult.BookData>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.AuthorName));

            CreateMap<Book, UpdateBookResult.BookData>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.AuthorName));

            CreateMap<Book, GetBookResult.BookData>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.AuthorName));

            CreateMap<Book, GetAllBooksResult.BookSummary>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.AuthorName));
        }
    }
}
