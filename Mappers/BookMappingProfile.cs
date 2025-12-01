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

            CreateMap<Book, CreateBookResult.BookData>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.AuthorName))
                .ForMember(dest => dest.Barcode, opt => opt.MapFrom(src => src.Barcode))
                .ForMember(dest => dest.Publisher, opt => opt.MapFrom(src => src.Publisher))
                .ForMember(dest => dest.ISBN, opt => opt.MapFrom(src => src.ISBN))
                .ForMember(dest => dest.SubjectGenre, opt => opt.MapFrom(src => src.SubjectGenre))
                .ForMember(dest => dest.PublicationDate, opt => opt.MapFrom(src => src.PublicationDate));

            CreateMap<Book, UpdateBookResult.BookData>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.AuthorName))
                .ForMember(dest => dest.Barcode, opt => opt.MapFrom(src => src.Barcode))
                .ForMember(dest => dest.Publisher, opt => opt.MapFrom(src => src.Publisher))
                .ForMember(dest => dest.ISBN, opt => opt.MapFrom(src => src.ISBN))
                .ForMember(dest => dest.SubjectGenre, opt => opt.MapFrom(src => src.SubjectGenre))
                .ForMember(dest => dest.PublicationDate, opt => opt.MapFrom(src => src.PublicationDate));

            CreateMap<Book, GetBookResult.BookData>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.AuthorName))
                .ForMember(dest => dest.Barcode, opt => opt.MapFrom(src => src.Barcode))
                .ForMember(dest => dest.Publisher, opt => opt.MapFrom(src => src.Publisher))
                .ForMember(dest => dest.ISBN, opt => opt.MapFrom(src => src.ISBN))
                .ForMember(dest => dest.SubjectGenre, opt => opt.MapFrom(src => src.SubjectGenre))
                .ForMember(dest => dest.PublicationDate, opt => opt.MapFrom(src => src.PublicationDate));

            CreateMap<Book, GetAllBooksResult.BookSummary>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.AuthorName))
                .ForMember(dest => dest.Barcode, opt => opt.MapFrom(src => src.Barcode))
                .ForMember(dest => dest.Publisher, opt => opt.MapFrom(src => src.Publisher))
                .ForMember(dest => dest.ISBN, opt => opt.MapFrom(src => src.ISBN))
                .ForMember(dest => dest.SubjectGenre, opt => opt.MapFrom(src => src.SubjectGenre))
                .ForMember(dest => dest.PublicationDate, opt => opt.MapFrom(src => src.PublicationDate)); 
        }
    }
}