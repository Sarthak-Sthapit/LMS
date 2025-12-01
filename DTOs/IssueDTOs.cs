namespace RestAPI.DTOs
{
    // For checkout
    public class CheckoutBookDto
    {
        public int BookId { get; set; }
        public int StudentId { get; set; }
        public int? BorrowDays { get; set; } = 14; // Optional, defaults to 14 days
    }

    // Keep existing DTOs for compatibility
    public class CreateIssueDto
    {
        public int BookId { get; set; }
        public int StudentId { get; set; }
    }

    public class UpdateIssueDto
    {
        public int? NewBookId { get; set; }
        public int? NewStudentId { get; set; }
    }
}