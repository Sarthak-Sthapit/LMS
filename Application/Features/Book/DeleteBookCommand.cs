using MediatR;

namespace RestAPI.Application.Commands
{
    public class DeleteBookCommand : IRequest<DeleteBookResult>
    {
        public int BookId { get; set; }
    }
    
    public class DeleteBookResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}