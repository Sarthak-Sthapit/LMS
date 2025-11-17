namespace RestAPI.Models
{
    public class Student
    {
        public int StudentId { get; set; } // Primary key
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string ContactNo { get; set; } = string.Empty;
        public string Faculty { get; set; } = string.Empty; 
        public string Semester { get; set; } = string.Empty; 
        public bool IsDeleted { get; set; } = false;
        
        // Navigation property - one student can have many issues
        public ICollection<Issue> Issues { get; set; } = new List<Issue>();
    }
}