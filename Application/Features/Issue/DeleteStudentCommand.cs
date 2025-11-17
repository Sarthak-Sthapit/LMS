using MediatR;

namespace RestAPI.Application.Commands
{
    public class DeleteIssueCommand : IRequest<DeleteIssueResult>
    {
        public int IssueId { get; set; }
    }
    
    public class DeleteIssueResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}