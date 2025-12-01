using MediatR;

namespace RestAPI.Application.Commands
{
   
    public class CheckoutBookCommand : IRequest<CheckoutBookResult>
    {
        public int BookId { get; set; }
        public int StudentId { get; set; }
        public int BorrowDays { get; set; } = 14; // Default 14 days
    }

    public class CheckoutBookResult
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
            public bool IsReturned { get; set; }
        }
    }

    
    public class ReturnBookCommand : IRequest<ReturnBookResult>
    {
        public int IssueId { get; set; }
    }

    public class ReturnBookResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int DaysOverdue { get; set; }
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
            public DateTime ReturnDate { get; set; }
            public bool IsOverdue { get; set; }
            public int DaysOverdue { get; set; }
        }
    }

   
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