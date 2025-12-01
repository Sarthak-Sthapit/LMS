
namespace RestAPI.Models
{
    public class Issue
    {
        public int IssueId { get; set; }
        public int BookId { get; set; }
        public int StudentId { get; set; }
        

        public DateTime IssueDate { get; set; } = DateTime.UtcNow;
        public DateTime DueDate { get; set; } // Auto-calculated: IssueDate + 14 days
        public DateTime? ReturnDate { get; set; } // Null if not returned yet
        
        public bool IsReturned { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        

        public bool IsOverdue => !IsReturned && DateTime.UtcNow > DueDate;
        public int DaysOverdue => IsOverdue ? (DateTime.UtcNow - DueDate).Days : 0;
        
        // Navigation properties
        public Book Book { get; set; } = null!;
        public Student Student { get; set; } = null!;
    }
}