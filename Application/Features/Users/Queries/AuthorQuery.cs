using MediatR;

namespace RestAPI.Application.Queries
{
    public class GetAuthorByIdQuery : IRequest<GetAuthorResult>
    {
        public int AuthorId { get; set; }
    }

    public class GetAllAuthorsQuery : IRequest<GetAllAuthorsResult> { }

    public class GetAuthorResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public AuthorData? Author { get; set; }

        public class AuthorData
        {
            public int AuthorId { get; set; }
            public string AuthorName { get; set; } = string.Empty;
        }
    }

    public class GetAllAuthorsResult
    {
        public bool Success { get; set; }
        public List<AuthorSummary> Authors { get; set; } = new();

        public class AuthorSummary
        {
            public int AuthorId { get; set; }
            public string AuthorName { get; set; } = string.Empty;
        }
    }
    

}
