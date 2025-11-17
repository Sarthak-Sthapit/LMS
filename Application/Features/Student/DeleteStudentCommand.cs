using MediatR;

namespace RestAPI.Application.Commands
{
    public class DeleteStudentCommand : IRequest<DeleteStudentResult>
    {
        public int StudentId { get; set; }
    }
    
    public class DeleteStudentResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
