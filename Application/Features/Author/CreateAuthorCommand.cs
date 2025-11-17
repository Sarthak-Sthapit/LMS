using MediatR;

namespace RestAPI.Application.Commands
{
    //command - what data we require for creating an author
    public class CreateAuthorCommand : IRequest<CreateAuthorResult>
    {
        public string AuthorName { get; set; } = string.Empty;
    }

    //Result - what the commmand returns
    public class CreateAuthorResult
    {
        public bool Success { get; set; }
        public int AuthorId { get; set; }
        public string Message { get; set; } = string.Empty;
        public AuthorData? Author { get; set; }

        public class AuthorData
        {
            public int AuthorId { get; set; }
            public string AuthorName { get; set; } = string.Empty;
        }
    }
}