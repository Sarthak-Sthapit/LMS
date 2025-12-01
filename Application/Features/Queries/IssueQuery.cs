using MediatR;

namespace RestAPI.Application.Queries
{
    public class GetIssueByIdQuery : IRequest<GetIssueResult>
    {
        public int IssueId { get; set; }
    }

    // Get all active loans 
    public class GetActiveLoansQuery : IRequest<GetActiveLoansResult>
    {
    }

    //  Get overdue books
    public class GetOverdueBooksQuery : IRequest<GetActiveLoansResult>
    {
    }

    public class GetAllIssuesQuery : IRequest<GetAllIssuesResult>
    {
    }

    public class GetIssuesByStudentQuery : IRequest<GetAllIssuesResult>
    {
        public int StudentId { get; set; }
    }

    public class GetIssuesByBookQuery : IRequest<GetAllIssuesResult>
    {
        public int BookId { get; set; }
    }

    public class GetIssueResult
    {
        public bool Success { get; set; }
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
            public DateTime DueDate { get; set; }
            public DateTime? ReturnDate { get; set; }
            public bool IsReturned { get; set; }
            public bool IsOverdue { get; set; }
            public int DaysOverdue { get; set; }
        }
    }

    public class GetAllIssuesResult
    {
        public bool Success { get; set; }
        public List<IssueSummary> Issues { get; set; } = new();
        
        public class IssueSummary
        {
            public int IssueId { get; set; }
            public int BookId { get; set; }
            public string BookTitle { get; set; } = string.Empty;
            public int StudentId { get; set; }
            public string StudentName { get; set; } = string.Empty;
            public DateTime IssueDate { get; set; }
            public DateTime DueDate { get; set; }
            public DateTime? ReturnDate { get; set; }
            public bool IsReturned { get; set; }
            public bool IsOverdue { get; set; }
            public int DaysOverdue { get; set; }
        }
    }

    // Result for active loans
    public class GetActiveLoansResult
    {
        public bool Success { get; set; }
        public List<LoanSummary> Loans { get; set; } = new();
        
        public class LoanSummary
        {
            public int IssueId { get; set; }
            public int BookId { get; set; }
            public string BookTitle { get; set; } = string.Empty;
            public int StudentId { get; set; }
            public string StudentName { get; set; } = string.Empty;
            public DateTime IssueDate { get; set; }
            public DateTime DueDate { get; set; }
            public bool IsOverdue { get; set; }
            public int DaysOverdue { get; set; }
        }
    }
}