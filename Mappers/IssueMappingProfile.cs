using AutoMapper;
using RestAPI.Models;
using RestAPI.Application.Commands;
using RestAPI.Application.Queries;

namespace RestAPI.Mappers
{
    public class IssueMappingProfile : Profile
    {
        public IssueMappingProfile()
        {
            CreateMap<Issue, CreateIssueResult.IssueData>()
                .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title))
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student.Name));

            CreateMap<Issue, UpdateIssueResult.IssueData>()
                .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title))
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student.Name));

            CreateMap<Issue, GetIssueResult.IssueData>()
                .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title))
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student.Name));

            CreateMap<Issue, GetAllIssuesResult.IssueSummary>()
                .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title))
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student.Name));
        }
    }
}