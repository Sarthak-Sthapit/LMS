using MediatR;

namespace RestAPI.Application.Commands
{
    public class CreateIssueCommand : IRequest<CreateIssueResult>
    {
        public int BookId { get; set; }
        public int StudentId { get; set; }
    }

    public class CreateIssueResult
    {
        public bool Success { get; set; }
        public int IssueId { get; set; }
        public string Message { get; set; } = string.Empty;
        public IssueData? Issue { get; set; }

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