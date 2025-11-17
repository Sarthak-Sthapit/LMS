namespace RestAPI.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int AuthorId { get; set; }
        public string Publisher { get; set; } = string.Empty;
        public string Barcode { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public string SubjectGenre { get; set; } = string.Empty;
        public DateTime? PublicationDate { get; set; }
        
        public bool IsDeleted { get; set; } = false;
        
        // Navigation properties
        public Author Author { get; set; } = null!;
        public ICollection<Issue> Issues { get; set; } = new List<Issue>();
    }
}