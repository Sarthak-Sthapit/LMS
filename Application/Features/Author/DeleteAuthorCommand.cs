using MediatR;

namespace RestAPI.Application.Commands
{
    public class DeleteAuthorCommand : IRequest<DeleteAuthorResult>
    {
        public int AuthorId { get; set; }
    }
    
    public class DeleteAuthorResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}