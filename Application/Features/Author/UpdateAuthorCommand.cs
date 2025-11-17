using MediatR;

namespace RestAPI.Application.Commands
{
    public class UpdateAuthorCommand : IRequest<UpdateAuthorResult>
    {
        public int AuthorId { get; set; }
        public string? NewAuthorName { get; set; }
    }

    public class UpdateAuthorResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public AuthorData? UpdatedAuthor { get; set; }
        
        public class AuthorData
        {
            public int AuthorId { get; set; }
            public string AuthorName { get; set; } = string.Empty;
        }
    }
}