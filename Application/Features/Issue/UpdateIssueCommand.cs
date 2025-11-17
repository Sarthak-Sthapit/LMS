using MediatR;

namespace RestAPI.Application.Commands
{
    public class UpdateIssueCommand : IRequest<UpdateIssueResult>
    {
        public int IssueId { get; set; }
        public int? NewBookId { get; set; }
        public int? NewStudentId { get; set; }
    }

    public class UpdateIssueResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public IssueData? UpdatedIssue { get; set; }
        
        public class IssueData
        {
            public int IssueId { get; set; }
            public int BookId { get; set; }
            public string BookTitle { get; set; } = string.Empty;
            public int StudentId { get; set; }
            public string StudentName { get; set; } = string.Empty;
            public DateTime IssueDate { get; set; }
        }
    }
}