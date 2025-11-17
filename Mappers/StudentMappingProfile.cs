using AutoMapper;
using RestAPI.Models;
using RestAPI.Application.Commands;
using RestAPI.Application.Queries;

namespace RestAPI.Mappers
{
    public class StudentMappingProfile : Profile
    {
        public StudentMappingProfile()
        {
            CreateMap<Student, CreateStudentResult.StudentData>();
            CreateMap<Student, UpdateStudentResult.StudentData>();
            CreateMap<Student, GetStudentResult.StudentData>();
            CreateMap<Student, GetAllStudentsResult.StudentSummary>();
        }
    }
}