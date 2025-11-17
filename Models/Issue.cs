namespace RestAPI.Models
{
    public class Issue
    {
        public int IssueId { get; set; }
        public int BookId { get; set; }
        public int StudentId { get; set; }
        public DateTime IssueDate { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        
        // Navigation properties - 
        public Book Book { get; set; } = null!; //null forgiving operator, does the same job just supressess nullable warnings
        public Student Student { get; set; } = null!;
    }
}