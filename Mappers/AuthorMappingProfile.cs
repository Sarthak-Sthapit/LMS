using AutoMapper;
using RestAPI.Models;
using RestAPI.Application.Commands;
using RestAPI.Application.Queries;

namespace RestAPI.Mappers
{
    public class AuthorMappingProfile : Profile
    {
        public AuthorMappingProfile()
        {
            // Author -> CreateAuthorResult.AuthorData
            CreateMap<Author, CreateAuthorResult.AuthorData>();

            // Author -> UpdateAuthorResult.AuthorData
            CreateMap<Author, UpdateAuthorResult.AuthorData>();

            // Author -> GetAuthorResult.AuthorData
            CreateMap<Author, GetAuthorResult.AuthorData>();

            // Author -> GetAllAuthorsResult.AuthorSummary
            CreateMap<Author, GetAllAuthorsResult.AuthorSummary>();
        }
    }
}