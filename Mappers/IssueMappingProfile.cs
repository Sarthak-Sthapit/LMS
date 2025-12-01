// Mappers/IssueMappingProfile.cs - COMPLETE WITH NEW MAPPINGS
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
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student.Name))
                .ForMember(dest => dest.IsOverdue, opt => opt.MapFrom(src => src.IsOverdue))
                .ForMember(dest => dest.DaysOverdue, opt => opt.MapFrom(src => src.DaysOverdue));

            CreateMap<Issue, GetAllIssuesResult.IssueSummary>()
                .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title))
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student.Name))
                .ForMember(dest => dest.IsOverdue, opt => opt.MapFrom(src => src.IsOverdue))
                .ForMember(dest => dest.DaysOverdue, opt => opt.MapFrom(src => src.DaysOverdue));

            //  Mappings for Checkout
            CreateMap<Issue, CheckoutBookResult.IssueData>()
                .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title))
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student.Name));

            //  Mappings for Return
            CreateMap<Issue, ReturnBookResult.IssueData>()
                .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title))
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student.Name))
                .ForMember(dest => dest.IsOverdue, opt => opt.MapFrom(src => src.IsOverdue))
                .ForMember(dest => dest.DaysOverdue, opt => opt.MapFrom(src => src.DaysOverdue));

            //  Mappings for Active Loans
            CreateMap<Issue, GetActiveLoansResult.LoanSummary>()
                .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title))
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student.Name))
                .ForMember(dest => dest.IsOverdue, opt => opt.MapFrom(src => src.IsOverdue))
                .ForMember(dest => dest.DaysOverdue, opt => opt.MapFrom(src => src.DaysOverdue));
        }
    }
}